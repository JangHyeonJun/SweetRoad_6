using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public float mouse_sensitivity = 10.0f;
    public GameObject particle;
    Index tile_index;
    Vector2 mouse_offset;

    private bool clicked;
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
        mouse_offset = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    }

    private void OnMouseDrag()
    {
        float dx = Input.mousePosition.x - mouse_offset.x;
        float dy = Input.mousePosition.y - mouse_offset.y;

        if (clicked)
        {
            Index swap_index = tile_index;
            if (dy > mouse_sensitivity)
                swap_index.y++;
            else if (dy < -mouse_sensitivity)
                swap_index.y--;
            else if (dx < -mouse_sensitivity)
                swap_index.x--;
            else if (dx > mouse_sensitivity)
                swap_index.x++;

            if (swap_index.x != tile_index.x || swap_index.y != tile_index.y)
            {
                clicked = false;
                StartCoroutine(GetComponentInParent<Board>().SwapTile(tile_index, swap_index));
            }
        }
    }

}
