using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelBeamStatic : MonoBehaviour
{

    [Header("Prefabs")]
    public GameObject beamLineRendererPrefab; //Put a prefab with a line renderer onto here.
    public GameObject beamStartPrefab; //This is a prefab that is put at the start of the beam.
    public GameObject beamEndPrefab; //Prefab put at end of beam.

    private GameObject beamStart;
    private GameObject beamEnd;
    private GameObject beam;
    //private LineRenderer line;
    //private LineRenderer line2nd;
    private LineRenderer[] line = new LineRenderer[10];

    [Header("Beam Options")]
    public bool alwaysOn = true; //Enable this to spawn the beam when script is loaded.
    public bool beamCollides = true; //Beam stops at colliders
    public float beamLength = 100; //Ingame beam length
    public float beamEndOffset = 0f; //How far from the raycast hit point the end effect is positioned
    public float textureScrollSpeed = 0f; //How fast the texture scrolls along the beam, can be negative or positive.
    public float textureLengthScale = 1f;   //Set this to the horizontal length of your texture relative to the vertical. 
                                            //Example: if texture is 200 pixels in height and 600 in length, set this to 3
    private Vector3[] original_Vertices; 
    bool MirrorCheck;
    Vector3[] end= new Vector3[10];
    RaycastHit[] hit = new RaycastHit[10];
    Vector3[] reflectDir=new Vector3[10];
    RaycastHit[] reflectHit=new RaycastHit[10];
    bool hit1st;
    int ReflectNumber;
    void Start()
    {
        //original_Vertices = new Vector3[line.positionCount];//original_Vertices�Ƃ���Vector3��錾����
    }

    private void OnEnable()
    {
        if (alwaysOn) //When the object this script is attached to is enabled, spawn the beam.
            SpawnBeam();
    }

    private void OnDisable() //If the object this script is attached to is disabled, remove the beam.
    {
        RemoveBeam();
    }

    void FixedUpdate()
    {
       
            
            
            //Vector3 end;
            //Vector3 beam2end;
            //RaycastHit hit;
            //RaycastHit hit2nd;
            

            //(�r�[�����R���W�����ƏՓ˂��邩�ǂ����̕ϐ�)��true&&�����Ă������ray���΂��q�b�g��̏����i�[���Ă�
            //�r�[���̃����_���[�̊J�n�������Œ�`�H
           
             BeamPosSet();

                

             ReflectBeamPosSet();

             
             ReflectBeamPosSet_after();

             for(int i=0;i<10;i++){
                 float distance = Vector3.Distance(transform.position, end[i-1]);
             line[i].material.mainTextureScale = new Vector2(distance / textureLengthScale, 1); //This sets the scale of the texture so it doesn't look stretched
             line[i].material.mainTextureOffset -= new Vector2(Time.deltaTime * textureScrollSpeed, 0); //This scrolls the texture along the beam if not set to 0
             }
            /* float distance = Vector3.Distance(transform.position, end[0]);
             line[0].material.mainTextureScale = new Vector2(distance / textureLengthScale, 1); //This sets the scale of the texture so it doesn't look stretched
             line[0].material.mainTextureOffset -= new Vector2(Time.deltaTime * textureScrollSpeed, 0); //This scrolls the texture along the beam if not set to 0
            
             float distance2 = Vector3.Distance(end[0], end[1]);
             line[1].material.mainTextureScale = new Vector2(distance2 / textureLengthScale, 1); //This sets the scale of the texture so it doesn't look stretched
             line[1].material.mainTextureOffset -= new Vector2(Time.deltaTime * textureScrollSpeed, 0); //This scrolls the texture along the beam if not set to 0
            */

           /*else{
                if (beamEnd)
             {
                beamEnd.transform.position = end;
                beamEnd.transform.LookAt(transform.position);
             }
             float distance = Vector3.Distance(transform.position, end);
             line.material.mainTextureScale = new Vector2(distance / textureLengthScale, 1); //This sets the scale of the texture so it doesn't look stretched
             line.material.mainTextureOffset -= new Vector2(Time.deltaTime * textureScrollSpeed, 0); //This scrolls the texture along the beam if not set to 0
            }*/



            
    }

    public void SpawnBeam() //This function spawns the prefab with linerenderer
    {
        if (beamLineRendererPrefab)
        {
            if (beamStartPrefab)
                beamStart = Instantiate(beamStartPrefab);
            if (beamEndPrefab)
                beamEnd = Instantiate(beamEndPrefab);
            /*beam = Instantiate(beamLineRendererPrefab);
            beam.transform.position = transform.position;
            beam.transform.parent = transform;
            beam.transform.rotation = transform.rotation;
            line[0] = beam.GetComponent<LineRenderer>();
            line[0].useWorldSpace = true;
            line[0].positionCount = 2;*/

            /*if (beamStartPrefab)
                beamStart = Instantiate(beamStartPrefab);
            if (beamEndPrefab)
                beamEnd = Instantiate(beamEndPrefab);*/
            for(int o=0;o<5;o++){
                beam = Instantiate(beamLineRendererPrefab);
            beam.transform.position = transform.position;
            beam.transform.parent = transform;
            beam.transform.rotation = transform.rotation;
            line[o] = beam.GetComponent<LineRenderer>();
            line[o].useWorldSpace = true;
            line[o].positionCount = 2;
            }    
            
                
        }
        else
            print("Add a hecking prefab with a line[0] renderer to the SciFiBeamStatic script on " + gameObject.name + "! Heck!");
    }

    public void RemoveBeam() //This function removes the prefab with linerenderer
    {
        if (beam)
            Destroy(beam);
        if (beamStart)
            Destroy(beamStart);
        if (beamEnd)
            Destroy(beamEnd);
    }



    public void BeamPosSet()
    {
        if (beamCollides && Physics.Raycast(transform.position, transform.forward, out hit[0]))
           {
               end[0] = hit[0].point - (transform.forward * beamEndOffset);
               if(hit[0].collider.tag=="Mirror")
                {
                    MirrorCheck=true;
                    hit1st=false;
                }
                if(hit[0].collider.tag!="Mirror"){
                    MirrorCheck=false;
                    hit1st=true;
                }
           } 
           //Checks for collision
                //end�ϐ��ɏՓ˒n�_(hit.point)����I�u�W�F�N�g�̐��ʃx�N�g���~�r�[���̏I�_����̃I�t�Z�b�g�l�̒l������
                //���炭Ray�̊J�n�ʒu����I�_�ʒu�������Ōv�Z���Ă���H          
            else{
                end[0] = transform.position + (transform.forward * beamLength);
                MirrorCheck=false;
            }
            line[0].SetPosition(0, transform.position);//�r�[���̊J�n�n�_���`
            line[0].SetPosition(1, end[0]);//�r�[���̏I���n�_���`
            
            if (beamStart)//�r�[���̔��˃G�t�F�N�g�̏ꏊ�ƌ���������
            {
                beamStart.transform.position = transform.position;
                beamStart.transform.LookAt(end[0]);
            }
            //�����܂�1�{�ڂ̏���
            
    }



    public void ReflectBeamPosSet()
    {
         if (beamCollides && Physics.Raycast(transform.position, transform.forward, out hit[1])) //Checks for collision beamCollides(�r�[�����R���W�����ƏՓ˂��邩�ǂ����̕ϐ�)��true&&�����Ă������ray���΂��q�b�g��̏����i�[���Ă�
             {
                 
                 reflectDir[0] = Vector3.Reflect(transform.forward, hit[1].normal);//ray�𔽎˂������x�N�g�����v�Z���i�[
                 if (Physics.Raycast(hit[1].point, reflectDir[0], out reflectHit[0]))//ray���J�n�n�_����I�_�܂Ŕ�΂��Ă���
                 {
                    end[1] = reflectHit[0].point - (reflectDir[0] * beamEndOffset);//��΂���ray�̏��i�[
                 }
                  else
                   end[1] = transform.position + (transform.forward * beamLength);
                
             }
              else
                end[1] = transform.position + (transform.forward * beamLength);
            
              
             
    }

    //�������傢���k�ł�����
    // public void Beam2nd_afterkari()
    // {
    //      for(int o=2;o<9;o++)
    //         {
                   
    //                 if(beamCollides && Physics.Raycast(end[o-1],reflectDir[o-2],out hit[o]))
    //                 {
    //                    reflectDir[o-1]=Vector3.Reflect(reflectDir[o-2],hit[o].normal);                    
    //                    if(Physics.Raycast(hit[o].point,reflectDir[o-1],out reflectHit[o-1]))
    //                    {
    //                     end[o]=reflectHit[o-1].point-(reflectDir[o-1]* beamEndOffset);
    //                     ReflectNumber++;

    //                     }
    //                 else
    //                 end[o]=new Vector3(0f,0f,0f);
    //                 //Debug.Log(end[o])
    //                 }
    //         }       
             
             
    //          for(int r=1;r<8;r++){
                  
    //          if(MirrorCheck==true&&hit[r].collider.tag=="Mirror")
    //          {
    //                 if(hit[r-1].collider.tag!="Mirror")
    //                 {
    //                     MirrorCheck=false;
    //                 }
    //                 else
    //                 {
    //                     line[r].SetPosition(0,end[r-1]);//���ˌ�̃r�[���̊J�n�n�_���`
    //                     line[r].SetPosition(1,end[r]);//���ˌ�̃r�[���̏I���n�_���`
    //                 if (beamEnd)
    //          {
    //             beamEnd.transform.position = end[r];
    //             beamEnd.transform.LookAt(end[r-1]);
    //          }
    //                 }
    //                 //line[r].SetPosition(0,end[r-1]);//���ˌ�̃r�[���̊J�n�n�_���`
    //                 //line[r].SetPosition(1,end[r]);//���ˌ�̃r�[���̏I���n�_���`

                    
             
    //          }
    //          else
    //             {
    //              line[r].SetPosition(0,new Vector3(0f,0f,0f));//���˂��Ȃ��ꍇ��2�{�ڂ̃r�[���̊J�n�n�_���`
    //               line[r].SetPosition(1,new Vector3(0f,0f,0f));//���˂��Ȃ��ꍇ��2�{�ڂ̃r�[���̏I���n�_���`
    //             }

    //           //1�{�ڂ̃r�[�������ɓ������Ă��Ȃ��Ƃ�
    //               if(hit1st==true){//
                 
    //               line[r].SetPosition(0,new Vector3(0f,0f,0f));//���˂��Ȃ��ꍇ��2�{�ڂ̃r�[���̊J�n�n�_���`
    //               line[r].SetPosition(1,new Vector3(0f,0f,0f));//���˂��Ȃ��ꍇ��2�{�ڂ̃r�[���̏I���n�_���`
    //               if (beamEnd)
    //              {
    //               beamEnd.transform.position = end[0];
    //               beamEnd.transform.LookAt(transform.position);
    //              }
    //             }
    //         }  
             
    // }
    public void ReflectBeamPosSet_after()
    {
        for(int p=2;p<9;p++)
        {
            if(beamCollides && Physics.Raycast(end[p-1],reflectDir[p-2],out hit[p]))
            {
                reflectDir[p-1]=Vector3.Reflect(reflectDir[p-2],hit[p].normal);                    
                if(Physics.Raycast(hit[p].point,reflectDir[p-1],out reflectHit[p-1]))
                    {
                        end[p]=reflectHit[p-1].point-(reflectDir[p-1]* beamEndOffset);
                        ReflectNumber++;

                    }
                    else
                    end[p]=new Vector3(0f,0f,0f);
                    //Debug.Log(end[o])
            }
            if(MirrorCheck==true&&hit[p-1].collider.tag=="Mirror")
            {
                if(hit[p-2].collider.tag!="Mirror")
                    {
                        MirrorCheck=false;
                    }
                else
                    {
                        line[p-1].SetPosition(0,end[p-2]);//���ˌ�̃r�[���̊J�n�n�_���`
                        line[p-1].SetPosition(1,end[p-1]);//���ˌ�̃r�[���̏I���n�_���`
                        if (beamEnd)
                          {
                          beamEnd.transform.position = end[p-1];
                          beamEnd.transform.LookAt(end[p-2]);
                          }
                    }
                    
            }
            else
                {
                 line[p-1].SetPosition(0,new Vector3(0f,0f,0f));//���˂��Ȃ��ꍇ��2�{�ڂ̃r�[���̊J�n�n�_���`
                  line[p-1].SetPosition(1,new Vector3(0f,0f,0f));//���˂��Ȃ��ꍇ��2�{�ڂ̃r�[���̏I���n�_���`
                }
             if(hit1st==true)
                {//
                 
                  line[p-1].SetPosition(0,new Vector3(0f,0f,0f));//���˂��Ȃ��ꍇ��2�{�ڂ̃r�[���̊J�n�n�_���`
                  line[p-1].SetPosition(1,new Vector3(0f,0f,0f));//���˂��Ȃ��ꍇ��2�{�ڂ̃r�[���̏I���n�_���`
                  if (beamEnd)
                 {
                  beamEnd.transform.position = end[0];
                  beamEnd.transform.LookAt(transform.position);
                 }
                }           
        }

    }
}