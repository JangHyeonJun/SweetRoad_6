using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    Index tile_index;
    private bool clicked;
    public float mouse_sensitivity = 10.0f;
    Vector2 mouse_offset;
    public void SetIndex(int x, int y)
    {
        tile_index.x = x;
        tile_index.y = y;
    }
    void Start()
    {
        clicked = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        clicked = true;
        print("[" + tile_index.y + "," + tile_index.x + "]");
        mouse_offset = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    }

    private void OnMouseDrag()
    {
        float dx = Input.mousePosition.x - mouse_offset.x;
        float dy = Input.mousePosition.y - mouse_offset.y;

        if (clicked)
        {
            Index move_index = tile_index;
            if (dy > mouse_sensitivity)
                move_index.y++;
            else if (dy < -mouse_sensitivity)
                move_index.y--;
            else if (dx < -mouse_sensitivity)
                move_index.x--;
            else if (dx > mouse_sensitivity)
                move_index.x++;

            if (move_index.x != tile_index.x || move_index.y != tile_index.y)
            {
                clicked = false;
                StartCoroutine(GetComponentInParent<Board>().MoveTile(tile_index, move_index));
            }
        }
    }

}
