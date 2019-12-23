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

    public IEnumerator MoveTile(Index tile_index, Index move_index)
    {
        if (!isMoving)
        {
            isMoving = true;
            if (move_index.y >= 0 && move_index.x >= 0 && move_index.y < board_size && move_index.x < board_size)
            {
                Sprite temp = tiles[tile_index.y, tile_index.x].GetComponent<SpriteRenderer>().sprite;
                tiles[tile_index.y, tile_index.x].GetComponent<SpriteRenderer>().sprite = tiles[move_index.y, move_index.x].GetComponent<SpriteRenderer>().sprite;
                tiles[move_index.y, move_index.x].GetComponent<SpriteRenderer>().sprite = temp;
            }
            yield return new WaitForSeconds(0.5f);
            if(!Match(tile_index) && !Match(move_index))
            {
                Sprite temp = tiles[tile_index.y, tile_index.x].GetComponent<SpriteRenderer>().sprite;
                tiles[tile_index.y, tile_index.x].GetComponent<SpriteRenderer>().sprite = tiles[move_index.y, move_index.x].GetComponent<SpriteRenderer>().sprite;
                tiles[move_index.y, move_index.x].GetComponent<SpriteRenderer>().sprite = temp;
            }
            isMoving = false;
        }
    }

    public bool Match(Index offset)
    {
        Sprite origin = tiles[offset.y, offset.x].GetComponent<SpriteRenderer>().sprite;
        List<Index> horizontal_indices = new List<Index>();
        List<Index> vertical_indices = new List<Index>();
        int i = 0;
        while (offset.x - i >= 0 && tiles[offset.y, offset.x - i].GetComponent<SpriteRenderer>().sprite == origin)
            horizontal_indices.Add(new Index(offset.x - i++, offset.y));
        i = 0;
        while (offset.x + i < board_size && tiles[offset.y, offset.x + i].GetComponent<SpriteRenderer>().sprite == origin)
            horizontal_indices.Add(new Index(offset.x + i++, offset.y));
        i = 0;
        while (offset.y - i >= 0 && tiles[offset.y - i, offset.x].GetComponent<SpriteRenderer>().sprite == origin)
            vertical_indices.Add(new Index(offset.x, offset.y - i++));
        i = 0;
        while (offset.y + i < board_size && tiles[offset.y + i, offset.x].GetComponent<SpriteRenderer>().sprite == origin)
            vertical_indices.Add(new Index(offset.x, offset.y + i++));

        if (horizontal_indices.Count > 2)
            for (int j = 0; i < horizontal_indices.Count; i++)
            {
                int x = horizontal_indices[j].x;
                int y = horizontal_indices[j].y;
                tiles[y, x].GetComponent<SpriteRenderer>().sprite = null;
            }

        if (vertical_indices.Count > 2)
            for (int j = 0; i < vertical_indices.Count; i++)
            {
                int x = vertical_indices[j].x;
                int y = vertical_indices[j].y;
                tiles[y, x].GetComponent<SpriteRenderer>().sprite = null;
            }

        return horizontal_indices.Count > 2 || vertical_indices.Count > 2;
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
                if (j - 2 > 0)
                    non_repeated_sprites.Remove(tiles[i, j - 2].GetComponent<SpriteRenderer>().sprite);
                if (i - 2 > 0)
                    non_repeated_sprites.Remove(tiles[i - 2, j].GetComponent<SpriteRenderer>().sprite);

                new_tile.GetComponent<SpriteRenderer>().sprite = non_repeated_sprites[Random.Range(0, non_repeated_sprites.Count)];
                new_tile.transform.parent = transform;
                new_tile.GetComponent<Tile>().SetIndex(j, i);
                tiles[i, j] = new_tile;
            }
        }
    }

}
