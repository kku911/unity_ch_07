using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

    public static float ACCELERATION = 1.0f;
    public static float SPEED_MIN = 2.0f;
    public static float SPEED_MAX = 3.0f;
    public static float JUMP_HEIGHT_MAX = 3.0f;
    public static float JUMP_KEY_RELEASE_REDUCE = 0.5f;

    public enum STEP {
        NONE = -1,
        RUN = 0,
        JUMP,
        MISS,
        NUM,
    }

    public STEP step = STEP.NONE;
    public STEP next_step = STEP.NONE;

    public float step_timer = 0.0f;
    public bool is_landed = false;
    public bool is_colided = false;
    public bool is_key_released = false;



    private void check_landed()
    {
        this.is_landed = false;
        do
        {
            Vector3 s = this.transform.position;
            Vector3 e = s + Vector3.down * 1.0f;
            RaycastHit hit;

            if (!Physics.Linecast(s, e, out hit))
            {
                break;
            }

            if (this.step == STEP.JUMP)
            {
                if (this.step_timer < Time.deltaTime * 3.0f)
                {
                    break;
                }
            }

            this.is_landed = true;

        } while (false);
    }

    // Use this for initialization
    void Start () {
        this.next_step = STEP.RUN;
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.Translate(new Vector3(3.0f * Time.deltaTime, 0.0f, 0.0f));

        Vector3 velocity = this.GetComponent<Rigidbody>().velocity;
        this.check_landed();
        this.step_timer += Time.deltaTime;

        if(this.next_step == STEP.NONE)
        {
            switch (this.step)
            {
                case STEP.RUN:
                    if (!this.is_landed)
                    {
                        //달리는 중이고 착지하지 않은 경우 아무것도 하지 않는다.
                    } else
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            //달리는 중이고 착지했고 왼쪽 버튼이 눌렸다면.
                            //다음 상태를 점프로 변경
                            this.next_step = STEP.JUMP;
                        }
                    }
                    break;
                case STEP.JUMP:
                    if (this.is_landed)
                    {
                        this.next_step = STEP.RUN;
                    }
                    break;
            }
        } 

        //'다음 정보'가 '상태 정보 없음'이 아닌 동안(상태가 변할 때만).
        while(this.next_step != STEP.NONE)
        {
            this.step = this.next_step;
            this.next_step = STEP.NONE;

            switch(this.step)
            {
                case STEP.JUMP:
                    velocity.y = Mathf.Sqrt(2.0f * 9.8f * PlayerControl.JUMP_HEIGHT_MAX);
                    this.is_key_released = false;
                break;
            }
        }

        switch (this.step)
        {
            case STEP.RUN:
                velocity.x += PlayerControl.ACCELERATION * Time.deltaTime;

                if(Mathf.Abs(this.GetComponent<Rigidbody>().velocity.x) > PlayerControl.SPEED_MAX)
                {
                    //최고 속도 제한 이하로 유지한다.
                    velocity.x *= PlayerControl.SPEED_MAX / Mathf.Sqrt(this.GetComponent<Rigidbody>().velocity.x);
                }
                break;

            case STEP.JUMP:

                do {
                    if (!Input.GetMouseButtonUp(0))
                    {
                        break;
                    }
                    if (this.is_key_released)
                    {
                        break;
                    }
                    if (velocity.y == 0.0f)
                    {
                        break;
                    }

                    velocity.y *= JUMP_KEY_RELEASE_REDUCE;

                    this.is_key_released = true;
                } while (false);

                break;
        }

        this.GetComponent<Rigidbody>().velocity = velocity;


    }//Update()
}


