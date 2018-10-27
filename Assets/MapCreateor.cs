using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreateor : MonoBehaviour {

    public static float BLOCK_WIDTH = 1.0f;
    public static float BLOCK_HEIGHT = 0.2f;
    public static int BLOCK_NUM_IN_SCREEN = 24;

    private struct FloorBlock
    {
        public bool is_created;
        public Vector3 postion;
    }

    private FloorBlock last_block;
    private PlayerControl player = null;
    private BlockCreator block_creator;

    ////FloorBlock fb;
    //FloorBlock.is_created = false;
    //FloorBlock.position = new Vector3(11.0f,1.0f,1.0f);

    // Use this for initialization
    void Start () {
	    this.player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
        this.last_block.is_created = false;
        this.block_creator = this.gameObject.GetComponent<BlockCreator>();
	}
	
	// Update is called once per frame
	void Update () {
        float block_generate_x = this.player.transform.position.x;

        block_generate_x += BLOCK_WIDTH * ((float)BLOCK_NUM_IN_SCREEN + 1) / 2.0f;

        while(this.last_block.postion.x < block_generate_x)
        {
            this.create_floor_block();
        }
	}

    private void create_floor_block()
    {
        Vector3 block_position;
        if(!this.last_block.is_created)
        {
            block_position = this.player.transform.position;
            block_position.x = BLOCK_WIDTH * ((float)BLOCK_NUM_IN_SCREEN / 2.0f);
            block_position.y = 0.0f;
        } else
        {
            block_position = this.last_block.postion;
        }

        block_position.x += BLOCK_WIDTH;

        this.block_creator.createBlock(block_position);

        this.last_block.postion = block_position;

        this.last_block.is_created = true;
    }

    public bool isDelete(GameObject block_object)
    {
        bool ret = false;

        float left_limit = this.player.transform.position.x - BLOCK_WIDTH * ((float)BLOCK_NUM_IN_SCREEN / 2.0f);

        if(block_object.transform.position.x < left_limit)
        {
            ret = true;
        }

        return (ret);
    }
}
