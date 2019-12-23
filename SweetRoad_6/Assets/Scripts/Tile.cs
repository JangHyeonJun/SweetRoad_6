using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private int tile_index_x, tile_index_y;
    private bool clicked;
    public float mouse_sensitivity = 10.0f;
    Vector2 mouse_offset;
    public void SetIndex(int x, int y)
    {
        tile_index_x = x;
        tile_index_y = y;
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
        mouse_offset = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    }

    private void OnMouseDrag()
    {
        float dx = Input.mousePosition.x - mouse_offset.x;
        float dy = Input.mousePosition.y - mouse_offset.y;

        if (clicked)
        {
            Board.Array drag_array = Board.Array.NONE;
            if (dy > mouse_sensitivity)
                drag_array = Board.Array.UP;
            else if (dy < -mouse_sensitivity)
                drag_array = Board.Array.DOWN;
            else if (dx < -mouse_sensitivity)
                drag_array = Board.Array.LEFT;
            else if (dx > mouse_sensitivity)
                drag_array = Board.Array.RIGHT;

            if (drag_array != Board.Array.NONE)
            {
                clicked = false;
                GetComponentInParent<Board>().MoveTile(tile_index_x, tile_index_y, drag_array);
            }
        }
    }

}
