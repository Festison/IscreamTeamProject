using KimKyeongHun;
using PangGom;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace YoungJaeKim
{
    public class Ghost : MonoBehaviour, IListenable
    {
        [SerializeField]
        Transform[] roamingPosition = new Transform[3];
        NavMeshAgent ghostAgent;
        public Animator ghostAnime;
        public Collider[] armCollider;
        [SerializeField]
        float checkTime = 1f;

        float chaseCount = 3f; // 순찰 n번하고 쫓아오기

        private bool is_SetPath = false;


        public float patrolable = 3f;
        public int currentpath = 0;
        public float roamingInterval = 10f;
        Vector3 targetPos;

        float loudness;
        public float Loudness { get => loudness; set => loudness = value; }

        AudioSource ghostRun;

        [SerializeField]
        Player loudPlayer;
        public Player LoudPlayer
        {
            get => loudPlayer;
            set
            {
                loudPlayer = value;
                if (loudPlayer != null && chaseCount >= 3f)
                {
                    isFind = true;
                    if (ghostRun == null)
                        ghostRun=SoundManager.Instance.PlayWaitingAudio(SoundManager.Instance.ghostRun, transform.position);//추격
                    else if (ghostRun.isPlaying == false)
                        ghostRun = null;
                    targetPos = value.transform.position;
                    ghostAgent.SetDestination(targetPos);
                }
                else
                    isFind = false;
            }
        }

        public Vector3 Pos
        {
            get => transform.position;
        }
        public bool isFind = false;

        public void TempFind()
        {

        }

        // Start is called before the first frame update
        void Start()
        {
            ListenerManager.Instance.listeners.Add(this);
            ghostAgent = GetComponent<NavMeshAgent>();
            ghostAnime = GetComponent<Animator>();
            StartCoroutine(GhostBehaviorCo());
        }


        IEnumerator GhostBehaviorCo()
        {
            while (true)
            {
                if(LoudPlayer != null && Vector3.Distance(targetPos, Pos) < 0.1f)
                {                   
                    LoudPlayer = null;
                    chaseCount = 0;
                }
                if (isFind == false)
                {
                    chaseCount++;
                    Debug.Log("로밍");
                    SoundManager.Instance.PlayAudio(SoundManager.Instance.ghostNormal, false, transform.position); // 평소
                    ghostAgent.SetDestination(roamingPosition[currentpath].position);
                    yield return new WaitForSeconds(roamingInterval);
                    if (isFind)
                        continue;


                    currentpath = currentpath + 1;
                    if (currentpath == roamingPosition.Length)
                        currentpath = 0;
                }

                yield return null;
            }
        }
        private void Update()
        {
            AnimeRun();

        }
        void Patrol()
        {
            if (!isFind) //&& ghostAgent.isStopped
            {
                StartCoroutine(Roaming());
            }
        }
        IEnumerator Roaming()
        {
            if (!is_SetPath)
            {
                Debug.Log("로밍");
                is_SetPath = true;
                ghostAgent.SetDestination(roamingPosition[currentpath].position);
                yield return new WaitForSeconds(roamingInterval);
                currentpath = currentpath + 1;
                if (currentpath == roamingPosition.Length)
                    currentpath = 0;
                is_SetPath = false;
            }
        }
        public void Next(int n)
        {
            ghostAgent.destination = roamingPosition[n].position;
        }
        void AnimeRun()
        {
            if (isFind)
            {
                ghostAnime.SetBool("Run", true);
            }
            else
            {
                ghostAnime.SetBool("Run", false);
            }
        }
        void AnimeAttack()
        {

        }
        private void OnTriggerEnter(Collider other)
        {
            Player player = other.gameObject.GetComponent<Player>();
            if (player != null)
            {
                //AnimeAttack();
                ghostAnime.SetBool("Attack", true);
                if (other is CharacterController)
                {
                    SoundManager.Instance.PlayAudio(SoundManager.Instance.ghostAttack, false, transform.position);
                    Debug.Log("공격!");
                    player.HpDown();
                    LoudPlayer = null;
                    chaseCount = 0;
                }
                else Debug.Log("빗나감!");
            }
        }
    }
}

