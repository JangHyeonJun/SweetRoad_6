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
    public Sprite munchkin_sprite;
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
    public IEnumerator SwapTile(Index tile_index, Index swap_index, float swap_delay = 0.3f)
    {
        if (!isMoving)
        {
            isMoving = true;
            if (swap_index.y >= 0 && swap_index.x >= 0 && swap_index.y < board_size && swap_index.x < board_size)
                SwapSprite(ref tiles[tile_index.y, tile_index.x], ref tiles[swap_index.y, swap_index.x]);

            yield return new WaitForSeconds(swap_delay);

            List<Index> matched_indices = new List<Index>();
            matched_indices.AddRange(Match(tile_index));
            matched_indices.AddRange(Match(swap_index));

            if (matched_indices.Count > 0)
            {
                for (int i = 0; i < matched_indices.Count; i++)
                    StartCoroutine(DropTile(matched_indices[i]));
            }
            else if (swap_index.y >= 0 && swap_index.x >= 0 && swap_index.y < board_size && swap_index.x < board_size)
            {
                SwapSprite(ref tiles[tile_index.y, tile_index.x], ref tiles[swap_index.y, swap_index.x]);
                yield return new WaitForSeconds(swap_delay);
                isMoving = false;
            }
        }
    }

    public IEnumerator DropTile(Index drop_index, float drop_delay = 0.2f)
    {
        isMoving = true;
        int x = drop_index.x;
        int y = drop_index.y;

        for (int i = 0; i < board_size;)
        {
            if (tiles[i, x].GetComponent<SpriteRenderer>().sprite == null)
            {
                for (int j = i; j < board_size - 1; j++)
                    SwapSprite(ref tiles[j, x], ref tiles[j + 1, x]);
                tiles[board_size - 1, x].GetComponent<SpriteRenderer>().sprite = tile_sprites[Random.Range(0, tile_sprites.Count)];
                Audio.instance.PlayDropSound();
                yield return new WaitForSeconds(drop_delay);
            }
            else
                i++; 
        }

        List<Index> matched_indices = new List<Index>();
        for (int k = 0; k < board_size; k++)
            matched_indices.AddRange(Match(new Index(x, k)));

        if (matched_indices.Count > 0)
        {
            for (int k = 0; k < matched_indices.Count; k++)
                StartCoroutine(DropTile(matched_indices[k]));
        }

        isMoving = false;
    }

    List<Index> GetSquareMatchedIndices(Index offset, Sprite origin)
    {
        List<Index> square_indices = new List<Index>();
        HashSet<Index> square_indices2 = new HashSet<Index>();
        if (offset.x > 0 && tiles[offset.y, offset.x - 1].GetComponent<SpriteRenderer>().sprite == origin) // left
        {
            if (offset.y < board_size - 1 && tiles[offset.y + 1, offset.x - 1] == origin) // up
            {
                square_indices.Add(new Index(offset.y, offset.x-1));
                square_indices.Add(new Index(offset.y+1, offset.x));
                square_indices.Add(new Index(offset.y+1, offset.x-1));
            }
            if (offset.y > 0 && tiles[offset.y - 1, offset.x - 1] == origin) // down
            {
                square_indices.Add(new Index(offset.y, offset.x - 1));
                square_indices.Add(new Index(offset.y - 1, offset.x));
                square_indices.Add(new Index(offset.y - 1, offset.x - 1));
            }
        }
        if (offset.x < board_size - 1 && tiles[offset.y, offset.x + 1].GetComponent<SpriteRenderer>().sprite == origin) // right
        {
            if (offset.y < board_size - 1 && tiles[offset.y + 1, offset.x + 1] == origin) // up
            {
                square_indices.Add(new Index(offset.y, offset.x + 1));
                square_indices.Add(new Index(offset.y + 1, offset.x));
                square_indices.Add(new Index(offset.y + 1, offset.x + 1));
            }
            if (offset.y > 0 && tiles[offset.y - 1, offset.x + 1] == origin) // down
            {
                square_indices.Add(new Index(offset.y, offset.x + 1));
                square_indices.Add(new Index(offset.y - 1, offset.x));
                square_indices.Add(new Index(offset.y - 1, offset.x + 1));
            }
        }

        square_indices = square_indices.Distinct<Index>().ToList();

        return square_indices;
    }
    List<Index> GetHorizontalMatchedIndices(Index offset, Sprite origin)
    {
        List<Index> horizontal_indices = new List<Index>();
        int i = 0;
        while (offset.x - i >= 0 && tiles[offset.y, offset.x - i].GetComponent<SpriteRenderer>().sprite == origin)
            horizontal_indices.Add(new Index(offset.x - i++, offset.y));
        i = 1;
        while (offset.x + i < board_size && tiles[offset.y, offset.x + i].GetComponent<SpriteRenderer>().sprite == origin)
            horizontal_indices.Add(new Index(offset.x + i++, offset.y));
        return horizontal_indices;
    }
    List<Index> GetVerticalMatchedIndices(Index offset, Sprite origin)
    {
        List<Index> vertical_indices = new List<Index>();

        int i = 0;
        while (offset.y - i >= 0 && tiles[offset.y - i, offset.x].GetComponent<SpriteRenderer>().sprite == origin)
            vertical_indices.Add(new Index(offset.x, offset.y - i++));
        i = 1;
        while (offset.y + i < board_size && tiles[offset.y + i, offset.x].GetComponent<SpriteRenderer>().sprite == origin)
            vertical_indices.Add(new Index(offset.x, offset.y + i++));

        return vertical_indices;
    }
    public List<Index> Match(Index offset)
    {
        Sprite origin = tiles[offset.y, offset.x].GetComponent<SpriteRenderer>().sprite;
        if (origin != null)
        {
            List<Index> horizontal_indices = GetHorizontalMatchedIndices(offset, origin);
            List<Index> vertical_indices = GetVerticalMatchedIndices(offset, origin);

            if (horizontal_indices.Count > 2)
                for (int j = 0; j < horizontal_indices.Count; j++)
                {
                    int x = horizontal_indices[j].x;
                    int y = horizontal_indices[j].y;
                    tiles[y, x].GetComponent<SpriteRenderer>().sprite = null;
                    tiles[y, x].GetComponent<Tile>().particle.GetComponent<ParticleSystem>().Play();
                }

            if (vertical_indices.Count > 2)
                for (int j = 0; j < vertical_indices.Count; j++)
                {
                    int x = vertical_indices[j].x;
                    int y = vertical_indices[j].y;
                    tiles[y, x].GetComponent<SpriteRenderer>().sprite = null;
                    tiles[y, x].GetComponent<Tile>().particle.GetComponent<ParticleSystem>().Play();
                }

            if (horizontal_indices.Count > 2 || vertical_indices.Count > 2)
            {
                Audio.instance.PlayMatchSound();
                return horizontal_indices;
            }
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
