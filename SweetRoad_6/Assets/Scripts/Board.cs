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
    public GameObject hole;
    public List<Sprite> tile_sprites;
    public Sprite munchkin_sprite;
    public Transform offset;

    private GameObject[,] tiles;
    private bool isMoving = false;

    // Start is called before the first frame update
    void Start()
    {
        CreateBoard();
        CreateHoles();
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

    public IEnumerator RollTile(Index offset, Array array, float roll_delay = 0.1f)
    {
        if (!isMoving && tiles[offset.y, offset.x].GetComponent<SpriteRenderer>().sprite == munchkin_sprite)
        {
            isMoving = true;
            GameManager.instance.move_num--;
            GameManager.instance.mission_num--;
            HashSet<Index> matched_cols = new HashSet<Index>();
            tiles[offset.y, offset.x].GetComponent<SpriteRenderer>().sprite = null;
            matched_cols.Add(new Index(offset.x, offset.y));
            switch (array)
            {
                case Array.UP:

                    for (int i = offset.y + 1; i < board_size; i++)
                    {
                        if (tiles[i, offset.x].GetComponent<SpriteRenderer>().sprite == munchkin_sprite)
                            GameManager.instance.mission_num--;
                        tiles[i, offset.x].GetComponent<SpriteRenderer>().sprite = munchkin_sprite;
                        yield return new WaitForSeconds(roll_delay);
                        tiles[i, offset.x].GetComponent<SpriteRenderer>().sprite = null;
                        tiles[i, offset.x].GetComponent<Tile>().particle.GetComponent<ParticleSystem>().Play();
                        GameManager.instance.score_num += 50;
                        Audio.instance.PlayMatchSound();
                        matched_cols.Add(new Index(offset.x, 0));

                    }
                    tiles[board_size - 1, offset.x].GetComponent<SpriteRenderer>().sprite = munchkin_sprite;
                    tiles[board_size - 1, offset.x].transform.
                        Translate(new Vector3(0, origin_tile.GetComponent<Renderer>().bounds.size.y, 0));
                    yield return new WaitForSeconds(roll_delay);
                    tiles[board_size - 1, offset.x].transform.
                        Translate(new Vector3(0, -origin_tile.GetComponent<Renderer>().bounds.size.y, 0));
                    tiles[board_size - 1, offset.x].GetComponent<SpriteRenderer>().sprite = null;
                    break;
                case Array.DOWN:
                    for (int i = offset.y - 1; i >= 0; i--)
                    {
                        if (tiles[i, offset.x].GetComponent<SpriteRenderer>().sprite == munchkin_sprite)
                            GameManager.instance.mission_num--;
                        tiles[i, offset.x].GetComponent<SpriteRenderer>().sprite = munchkin_sprite;
                        yield return new WaitForSeconds(roll_delay);
                        tiles[i, offset.x].GetComponent<SpriteRenderer>().sprite = null;
                        tiles[i, offset.x].GetComponent<Tile>().particle.GetComponent<ParticleSystem>().Play();
                        GameManager.instance.score_num += 50;
                        Audio.instance.PlayMatchSound();
                        matched_cols.Add(new Index(offset.x, 0));

                    }
                    tiles[0, offset.x].GetComponent<SpriteRenderer>().sprite = munchkin_sprite;
                    tiles[0, offset.x].transform.
                        Translate(new Vector3(0, -origin_tile.GetComponent<Renderer>().bounds.size.y, 0));
                    yield return new WaitForSeconds(roll_delay);
                    tiles[0, offset.x].transform.
                        Translate(new Vector3(0, origin_tile.GetComponent<Renderer>().bounds.size.y, 0));
                    tiles[0, offset.x].GetComponent<SpriteRenderer>().sprite = null;
                    break;
                case Array.LEFT:
                    for (int i = offset.x - 1; i >= 0; i--)
                    {
                        if (tiles[offset.y, i].GetComponent<SpriteRenderer>().sprite == munchkin_sprite)
                            GameManager.instance.mission_num--;
                        tiles[offset.y, i].GetComponent<SpriteRenderer>().sprite = munchkin_sprite;
                        yield return new WaitForSeconds(roll_delay);
                        tiles[offset.y, i].GetComponent<SpriteRenderer>().sprite = null;
                        tiles[offset.y, i].GetComponent<Tile>().particle.GetComponent<ParticleSystem>().Play();
                        GameManager.instance.score_num += 50;
                        Audio.instance.PlayMatchSound();
                        matched_cols.Add(new Index(i, 0));

                    }
                    tiles[offset.y, 0].GetComponent<SpriteRenderer>().sprite = munchkin_sprite;
                    tiles[offset.y, 0].transform.
                        Translate(new Vector3(-origin_tile.GetComponent<Renderer>().bounds.size.x, 0, 0));
                    yield return new WaitForSeconds(roll_delay);
                    tiles[offset.y, 0].transform.
                        Translate(new Vector3(origin_tile.GetComponent<Renderer>().bounds.size.x, 0, 0));
                    tiles[offset.y, 0].GetComponent<SpriteRenderer>().sprite = null;
                    break;
                case Array.RIGHT:
                    for (int i = offset.x + 1; i < board_size; i++)
                    {
                        if (tiles[offset.y, i].GetComponent<SpriteRenderer>().sprite == munchkin_sprite)
                            GameManager.instance.mission_num--;
                        tiles[offset.y, i].GetComponent<SpriteRenderer>().sprite = munchkin_sprite;
                        yield return new WaitForSeconds(roll_delay);
                        tiles[offset.y, i].GetComponent<SpriteRenderer>().sprite = null;
                        tiles[offset.y, i].GetComponent<Tile>().particle.GetComponent<ParticleSystem>().Play();
                        GameManager.instance.score_num += 50;
                        Audio.instance.PlayMatchSound();
                        matched_cols.Add(new Index(i, 0));
                    }
                    tiles[offset.y, board_size - 1].GetComponent<SpriteRenderer>().sprite = munchkin_sprite;
                    tiles[offset.y, board_size - 1].transform.
                        Translate(new Vector3(origin_tile.GetComponent<Renderer>().bounds.size.x, 0, 0));
                    yield return new WaitForSeconds(roll_delay);
                    tiles[offset.y, board_size - 1].transform.
                        Translate(new Vector3(-origin_tile.GetComponent<Renderer>().bounds.size.x, 0, 0));
                    tiles[offset.y, board_size - 1].GetComponent<SpriteRenderer>().sprite = null;
                    break;
                default:
                    isMoving = true;
                    break;
            }
            foreach (Index i in matched_cols)
                StartCoroutine(DropTile(i));
        }
    }

    public IEnumerator SwapTile(Index tile_index, Index swap_index, float swap_delay = 0.3f)
    {
        if (!isMoving)
        {
            isMoving = true;
            if (swap_index.y >= 0 && swap_index.x >= 0 && swap_index.y < board_size && swap_index.x < board_size)
            {
                SwapSprite(ref tiles[tile_index.y, tile_index.x], ref tiles[swap_index.y, swap_index.x]);

                yield return new WaitForSeconds(swap_delay);

                HashSet<Index> matched_indices = new HashSet<Index>();
                matched_indices.UnionWith(Match(tile_index));
                matched_indices.UnionWith(Match(swap_index));

                if (matched_indices.Count > 0)
                {
                    GameManager.instance.move_num--;
                    HashSet<Index> matched_cols = ClearTiles(matched_indices);
                    yield return new WaitForSeconds(swap_delay);
                    foreach (Index i in matched_cols)
                        StartCoroutine(DropTile(i));
                }
                else
                {
                    SwapSprite(ref tiles[tile_index.y, tile_index.x], ref tiles[swap_index.y, swap_index.x]);
                    yield return new WaitForSeconds(swap_delay);
                    isMoving = false;
                }
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

        //        yield return new WaitForSeconds(0.8f);

        HashSet<Index> matched_indices = new HashSet<Index>();
        for (int i = 0; i < board_size; i++)
            matched_indices.UnionWith(Match(new Index(x, i)));

        HashSet<Index> matched_cols = ClearTiles(matched_indices);
        foreach (Index i in matched_cols)
            StartCoroutine(DropTile(i));

        isMoving = false;
    }

    HashSet<Index> GetSquareMatchedIndices(Index offset, Sprite origin)
    {
        HashSet<Index> square_indices = new HashSet<Index>();
        if (origin != munchkin_sprite)
        {
            square_indices.Add(new Index(offset.x, offset.y));
            if (offset.x > 0 && tiles[offset.y, offset.x - 1].GetComponent<SpriteRenderer>().sprite == origin) // left
            {
                if (offset.y < board_size - 1 && tiles[offset.y + 1, offset.x].GetComponent<SpriteRenderer>().sprite == origin &&
                    offset.y < board_size - 1 && tiles[offset.y + 1, offset.x - 1].GetComponent<SpriteRenderer>().sprite == origin) // up
                {
                    square_indices.Add(new Index(offset.x - 1, offset.y));
                    square_indices.Add(new Index(offset.x, offset.y + 1));
                    square_indices.Add(new Index(offset.x - 1, offset.y + 1));
                }
                if (offset.y > 0 && tiles[offset.y - 1, offset.x].GetComponent<SpriteRenderer>().sprite == origin &&
                    offset.y > 0 && tiles[offset.y - 1, offset.x - 1].GetComponent<SpriteRenderer>().sprite == origin) // down
                {
                    square_indices.Add(new Index(offset.x - 1, offset.y));
                    square_indices.Add(new Index(offset.x, offset.y - 1));
                    square_indices.Add(new Index(offset.x - 1, offset.y - 1));
                }
            }
            if (offset.x < board_size - 1 && tiles[offset.y, offset.x + 1].GetComponent<SpriteRenderer>().sprite == origin) // right
            {
                if (offset.y < board_size - 1 && tiles[offset.y + 1, offset.x].GetComponent<SpriteRenderer>().sprite == origin &&
                    offset.y < board_size - 1 && tiles[offset.y + 1, offset.x + 1].GetComponent<SpriteRenderer>().sprite == origin) // up
                {
                    square_indices.Add(new Index(offset.x + 1, offset.y));
                    square_indices.Add(new Index(offset.x, offset.y + 1));
                    square_indices.Add(new Index(offset.x + 1, offset.y + 1));
                }
                if (offset.y > 0 && tiles[offset.y - 1, offset.x].GetComponent<SpriteRenderer>().sprite == origin &&
                    offset.y > 0 && tiles[offset.y - 1, offset.x + 1].GetComponent<SpriteRenderer>().sprite == origin) // down
                {
                    square_indices.Add(new Index(offset.x + 1, offset.y));
                    square_indices.Add(new Index(offset.x, offset.y - 1));
                    square_indices.Add(new Index(offset.x + 1, offset.y - 1));
                }
            }
        }
        return square_indices;
    }
    HashSet<Index> GetHorizontalMatchedIndices(Index offset, Sprite origin)
    {
        HashSet<Index> horizontal_indices = new HashSet<Index>();
        if (origin != munchkin_sprite)
        {
            int i = 0;
            while (offset.x - i >= 0 && tiles[offset.y, offset.x - i].GetComponent<SpriteRenderer>().sprite == origin)
                horizontal_indices.Add(new Index(offset.x - i++, offset.y));
            i = 1;
            while (offset.x + i < board_size && tiles[offset.y, offset.x + i].GetComponent<SpriteRenderer>().sprite == origin)
                horizontal_indices.Add(new Index(offset.x + i++, offset.y));
        }
        return horizontal_indices;
    }
    HashSet<Index> GetVerticalMatchedIndices(Index offset, Sprite origin)
    {
        HashSet<Index> vertical_indices = new HashSet<Index>();
        if (origin != munchkin_sprite)
        {
            int i = 0;
            while (offset.y - i >= 0 && tiles[offset.y - i, offset.x].GetComponent<SpriteRenderer>().sprite == origin)
                vertical_indices.Add(new Index(offset.x, offset.y - i++));
            i = 1;
            while (offset.y + i < board_size && tiles[offset.y + i, offset.x].GetComponent<SpriteRenderer>().sprite == origin)
                vertical_indices.Add(new Index(offset.x, offset.y + i++));
        }
        return vertical_indices;
    }

    HashSet<Index> ClearTiles(HashSet<Index> matched_indices)
    {
        HashSet<Index> matched_cols = new HashSet<Index>();
        if (matched_indices.Count > 0)
        {
            foreach (Index i in matched_indices)
            {
                tiles[i.y, i.x].GetComponent<SpriteRenderer>().sprite = null;
                tiles[i.y, i.x].GetComponent<Tile>().particle.GetComponent<ParticleSystem>().Play();
                GameManager.instance.score_num += 50;
                matched_cols.Add(new Index(i.x, 0));
            }
            Audio.instance.PlayMatchSound();
        }

        return matched_cols;
    }
    public HashSet<Index> Match(Index offset)
    {
        Sprite origin = tiles[offset.y, offset.x].GetComponent<SpriteRenderer>().sprite;
        if (origin != null)
        {
            HashSet<Index> horizontal_indices = GetHorizontalMatchedIndices(offset, origin);
            HashSet<Index> vertical_indices = GetVerticalMatchedIndices(offset, origin);
            HashSet<Index> square_indices = GetSquareMatchedIndices(offset, origin);

            HashSet<Index> matched_indices = new HashSet<Index>();

            if (horizontal_indices.Count >= 3)
                matched_indices.UnionWith(horizontal_indices);
            if (vertical_indices.Count >= 3)
                matched_indices.UnionWith(vertical_indices);
            if (square_indices.Count >= 4)
            {
                matched_indices.UnionWith(square_indices);
                matched_indices.Remove(offset);
                GameManager.instance.score_num += 50;
                tiles[offset.y, offset.x].GetComponent<SpriteRenderer>().sprite = munchkin_sprite;
            }

            if (matched_indices.Count > 0)
            {
                Audio.instance.PlayMatchSound();
                return matched_indices;
            }
            else
                return new HashSet<Index>();
        }
        else
            return new HashSet<Index>();
    }

    void CreateBoard()
    {
        tiles = new GameObject[board_size, board_size];
        for (int i = 0; i < board_size; i++)
        {
            for (int j = 0; j < board_size; j++)
            {
                Vector2 render_size = origin_tile.GetComponent<Renderer>().bounds.size;
                GameObject new_tile = Instantiate(origin_tile, new Vector3(offset.position.x + render_size.x * j,
                    offset.position.y + render_size.y * i, 0), origin_tile.transform.rotation);

                List<Sprite> non_repeated_sprites = new List<Sprite>(tile_sprites);

                // except 3-match case
                if (j - 2 >= 0)
                    non_repeated_sprites.Remove(tiles[i, j - 2].GetComponent<SpriteRenderer>().sprite);
                if (i - 2 >= 0)
                    non_repeated_sprites.Remove(tiles[i - 2, j].GetComponent<SpriteRenderer>().sprite);
                // except 2by2 match case
                if (j - 1 >= 0 && i - 1 >= 0)
                    non_repeated_sprites.Remove(tiles[i - 1, j - 1].GetComponent<SpriteRenderer>().sprite);

                new_tile.GetComponent<SpriteRenderer>().sprite = non_repeated_sprites[Random.Range(0, non_repeated_sprites.Count)];
                new_tile.transform.parent = transform;
                new_tile.GetComponent<Tile>().SetIndex(j, i);
                tiles[i, j] = new_tile;
            }
        }
    }

    void CreateHoles(int z_pos = 1)
    {
        Vector2 render_size = hole.GetComponent<Renderer>().bounds.size;
        for (int i = 0; i < board_size; i++)
            Instantiate(hole, new Vector3(offset.position.x + render_size.x * i,
                offset.position.y + render_size.y * board_size, z_pos), hole.transform.rotation).transform.parent = transform;
        for (int i = 0; i < board_size; i++)
            Instantiate(hole, new Vector3(offset.position.x + render_size.x * i,
                offset.position.y - render_size.y, z_pos), hole.transform.rotation).transform.parent = transform;
        for (int i = 0; i < board_size; i++)
            Instantiate(hole, new Vector3(offset.position.x - render_size.x,
                offset.position.y + render_size.y * i, z_pos), hole.transform.rotation).transform.parent = transform;
        for (int i = 0; i < board_size; i++)
            Instantiate(hole, new Vector3(offset.position.x + render_size.x * board_size,
                offset.position.y + render_size.y * i, z_pos), hole.transform.rotation).transform.parent = transform;
    }
}
