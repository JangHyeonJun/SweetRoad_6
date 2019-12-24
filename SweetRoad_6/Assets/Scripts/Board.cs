using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Index
{
    public int x, y;
    public Index(int _x, int _y) { x = _x; y = _y; }
}
public class Board : MonoBehaviour
{
    public enum Array { UP, DOWN, LEFT, RIGHT, NONE };
    const int board_size = 7;
    public GameObject origin_tile;
    public List<Sprite> tile_sprites;
    public Transform offset;

    private GameObject[,] tiles;
    private bool isMoving = false;

    // Start is called before the first frame update
    void Start()
    {
        CreateBoard();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SwapSprite(ref GameObject a, ref GameObject b)
    {
        Sprite temp = a.GetComponent<SpriteRenderer>().sprite;
        a.GetComponent<SpriteRenderer>().sprite = b.GetComponent<SpriteRenderer>().sprite;
        b.GetComponent<SpriteRenderer>().sprite = temp;
    }
    public IEnumerator SwapTile(Index tile_index, Index swap_index)
    {
        if (!isMoving)
        {
            isMoving = true;
            if (swap_index.y >= 0 && swap_index.x >= 0 && swap_index.y < board_size && swap_index.x < board_size)
                SwapSprite(ref tiles[tile_index.y, tile_index.x], ref tiles[swap_index.y, swap_index.x]);

            yield return new WaitForSeconds(0.5f);

            List<Index> matched_indices= new List<Index>();
            matched_indices.AddRange(Match(tile_index));
            matched_indices.AddRange(Match(swap_index));

            if (matched_indices.Count > 0)
            {
                for (int i = 0; i < matched_indices.Count; i++)
                    StartCoroutine(DropTile(matched_indices[i]));
                isMoving = false;
            }
            else if (swap_index.y >= 0 && swap_index.x >= 0 && swap_index.y < board_size && swap_index.x < board_size)
            {
                SwapSprite(ref tiles[tile_index.y, tile_index.x], ref tiles[swap_index.y, swap_index.x]);
                isMoving = false;
            }
        }
    }

    public IEnumerator DropTile(Index drop_index)
    {
        int x = drop_index.x;
        int y = drop_index.y;
        int i = 0;
        int null_tile_count = 0;
        // check up
        while (y + i < board_size && tiles[y + i++, x].GetComponent<SpriteRenderer>().sprite == null)
            null_tile_count++;
        // check down
        while (y > 0 && tiles[y - 1, x].GetComponent<SpriteRenderer>().sprite == null)
        {
            y--;
            null_tile_count++;
        }

        if (null_tile_count > 0)
            isMoving = true;

        for (i=0; i<null_tile_count; i++)
        {
            for (int j = y + 1; j < board_size; j++)
                tiles[j - 1, x].GetComponent<SpriteRenderer>().sprite = tiles[j, x].GetComponent<SpriteRenderer>().sprite;
            tiles[board_size - 1, x].GetComponent<SpriteRenderer>().sprite = tile_sprites[Random.Range(0, tile_sprites.Count)];
            yield return new WaitForSeconds(0.3f);
        }

        if (null_tile_count > 0)
            isMoving = false;

        yield return new WaitForSeconds(1.0f);

        for (int k = 0; k < board_size; k++)
            Match(new Index(k, x));
    }

    public List<Index> Match(Index offset)
    {
        Sprite origin = tiles[offset.y, offset.x].GetComponent<SpriteRenderer>().sprite;
        if (origin != null)
        {
            List<Index> horizontal_indices = new List<Index>();
            List<Index> vertical_indices = new List<Index>();
            horizontal_indices.Add(new Index(offset.x, offset.y));
            vertical_indices.Add(new Index(offset.x, offset.y));

            int i = 1;
            while (offset.x - i >= 0 && tiles[offset.y, offset.x - i].GetComponent<SpriteRenderer>().sprite == origin)
                horizontal_indices.Add(new Index(offset.x - i++, offset.y));
            i = 1;
            while (offset.x + i < board_size && tiles[offset.y, offset.x + i].GetComponent<SpriteRenderer>().sprite == origin)
                horizontal_indices.Add(new Index(offset.x + i++, offset.y));
            i = 1;
            while (offset.y - i >= 0 && tiles[offset.y - i, offset.x].GetComponent<SpriteRenderer>().sprite == origin)
                vertical_indices.Add(new Index(offset.x, offset.y - i++));
            i = 1;
            while (offset.y + i < board_size && tiles[offset.y + i, offset.x].GetComponent<SpriteRenderer>().sprite == origin)
                vertical_indices.Add(new Index(offset.x, offset.y + i++));

            if (horizontal_indices.Count > 2)
                for (int j = 0; j < horizontal_indices.Count; j++)
                {
                    int x = horizontal_indices[j].x;
                    int y = horizontal_indices[j].y;
                    tiles[y, x].GetComponent<SpriteRenderer>().sprite = null;
                }

            if (vertical_indices.Count > 2)
                for (int j = 0; j < vertical_indices.Count; j++)
                {
                    int x = vertical_indices[j].x;
                    int y = vertical_indices[j].y;
                    tiles[y, x].GetComponent<SpriteRenderer>().sprite = null;
                }

            if (horizontal_indices.Count > 2 || vertical_indices.Count > 2)
                return horizontal_indices;
            else
                return new List<Index>();
        }
        else
            return new List<Index>();
    }

    private void CreateBoard()
    {
        tiles = new GameObject[board_size, board_size];
        for (int i = 0; i < board_size; i++)
        {
            for (int j = 0; j < board_size; j++)
            {
                Vector2 render_size = origin_tile.GetComponent<Renderer>().bounds.size;
                GameObject new_tile = Instantiate(origin_tile, new Vector3(offset.position.x + render_size.x * j,
                    offset.position.y + render_size.y * i, 0), transform.rotation);

                List<Sprite> non_repeated_sprites = new List<Sprite>(tile_sprites);
                if (j - 2 >= 0)
                    non_repeated_sprites.Remove(tiles[i, j - 2].GetComponent<SpriteRenderer>().sprite);
                if (i - 2 >= 0)
                    non_repeated_sprites.Remove(tiles[i - 2, j].GetComponent<SpriteRenderer>().sprite);

                new_tile.GetComponent<SpriteRenderer>().sprite = non_repeated_sprites[Random.Range(0, non_repeated_sprites.Count)];
                new_tile.transform.parent = transform;
                new_tile.GetComponent<Tile>().SetIndex(j, i);
                tiles[i, j] = new_tile;
            }
        }
    }

}
