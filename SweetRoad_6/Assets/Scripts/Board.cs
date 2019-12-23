using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public enum Array { UP, DOWN, LEFT, RIGHT, NONE };
    const int board_size = 7;
    public GameObject origin_tile;
    public List<Sprite> tile_sprites;
    public Transform offset;

    private GameObject[,] tiles;

    // Start is called before the first frame update
    void Start()
    {
        CreateBoard();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool MoveTile(int x, int y, Array array)
    {
        int dx, dy;
        switch (array)
        {
            case Array.UP:
                dx = x; dy = y + 1;
                break;
            case Array.DOWN:
                dx = x; dy = y - 1;
                break;
            case Array.LEFT:
                dx = x - 1; dy = y;
                break;
            case Array.RIGHT:
                dx = x + 1; dy = y;
                break;
            default:
                dx = x; dy = y;
                break;
        }
        if (dy >= 0 && dx >= 0 && dy < board_size && dx < board_size)
        {
            Sprite temp = tiles[y, x].GetComponent<SpriteRenderer>().sprite;
            tiles[y, x].GetComponent<SpriteRenderer>().sprite = tiles[dy, dx].GetComponent<SpriteRenderer>().sprite;
            tiles[dy, dx].GetComponent<SpriteRenderer>().sprite = temp;

            return true;
        }

        return false;
    }

    public bool matched(GameObject tile, Array array)
    {
        switch (array)
        {
            case Array.UP:
                break;
            case Array.DOWN:
                break;
            case Array.LEFT:
                break;
            case Array.RIGHT:
                break;
            default:
                break;
        }
        return true;
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
