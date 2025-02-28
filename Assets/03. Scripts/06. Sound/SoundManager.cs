using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace PangGom
{
    public class SoundManager : Singleton<SoundManager>
    {
        public AudioSource curBgm;
        public float volume = 1;
        [SerializeField]
        float initLength;
        public GameObject audioSourcePref;
        public AudioClip playerFootSound;//플레이어 발소리ok
        public AudioClip playerDamage;//플레이어가 데미지를 받았을 때 ok
        public AudioClip playerDead;//플레이어가 죽었을 때ok
        public AudioClip itemGet;//아이템 획득ok
        public AudioClip ghostNormal;//귀신 평소 소리
        public AudioClip ghostAttack;//귀신 공격 소리
        public AudioClip ghostRun;//귀신이 추격할 때
        public AudioClip heartBeat;//심박 소리
        public AudioClip doorOpen;//문 여는 소리ok
        public AudioClip doorClose;//문 닫는 소리ok
        public AudioClip toilelDoorOpen;//화장실 문 여는 소리
        public AudioClip toilelDoorClose;//화장실문 닫는 소리
        public AudioClip steelDoorOpen;//철문 여는 소리
        public AudioClip steelDoorClose;//철문 닫는 소리
        public AudioClip waterDrop;//물떨어지는 소리ok
        public AudioClip duck;//오리 소리
        public AudioClip duckRun;//오리 발광 소리
        public AudioClip toilelEventFoot;//화장실 이벤트 발소리
        public AudioClip toilelEventHumming;//화장실 이벤트 허밍
        public AudioClip shhSound;//쉿 소리
        public AudioClip girlLaughSound;//웃음소리
        public AudioClip womanLaughSound;//웃음소리ok
        public AudioClip solveSound;//잠금해제 효과음ok
        public AudioClip mechKeybord;//퍼즐맞추는 효과음1ok
        public AudioClip deadBodyEvent;//DeadBodyEvent ok
        public AudioClip leverPullsound;//레버 클릭 효과음
        public AudioClip leverFailsound;//레버 실패 효과음
        public AudioClip leverSucesssound;//레버 성공 효과음
        public AudioClip detectiveSound;//탐지기 탐지 효과음
        public AudioClip bookOpen;// 책 여는 효과음

        public AudioClip dialSolved;

        Queue<GameObject> soundQueue = new Queue<GameObject>();

        // 풀을 초기화하는 메서드
        protected virtual void Init()
        {
            InitQueue();
        }

        void InitQueue()
        {
            for (int i = 0; i < initLength; i++)
            {
                CreateObj();
            }
        }
        void CreateObj()
        {
            GameObject obj = Instantiate(audioSourcePref);
            obj.transform.SetParent(this.transform);
            obj.SetActive(false);
            soundQueue.Enqueue(obj);
        }

        public AudioSource PopObj()
        {
            if (soundQueue.Count == 0)
                CreateObj();
            GameObject returnObj = soundQueue.Dequeue();
            returnObj.SetActive(true);
            returnObj.transform.SetParent(null);
            return returnObj.GetComponent<AudioSource>();
        }
        public void ReturnObj(GameObject obj)
        {
            soundQueue.Enqueue(obj.gameObject);
            obj.SetActive(false);
            obj.transform.parent = this.transform;
        }
        public void PlayAudio(AudioClip clip, bool isLoop)
        {
            AudioSource audio = PopObj();
            audio.clip = clip;
            audio.loop = isLoop;        
            audio.volume = volume;
            audio.PlayOneShot(clip);
        }

        public AudioSource PlayWaitingAudio(AudioClip clip)
        {
            AudioSource audio = PopObj();
            audio.clip = clip;         
            audio.volume = volume;
            audio.PlayOneShot(clip);
            return audio;
        }
        public AudioSource PlayWaitingAudio(AudioClip clip, Vector3 pos)
        {
            AudioSource audio = PopObj();
            audio.clip = clip;
            audio.transform.position = pos;
            audio.volume = volume;
            audio.PlayOneShot(clip);
            return audio;
        }

        public void PlayAudio(AudioClip clip, bool isLoop, Vector3 pos)
        {
            AudioSource audio = PopObj();
            audio.transform.position = pos;
            audio.clip = clip;
            audio.loop = isLoop;
            audio.spatialBlend = 1.0f;
            audio.volume = volume;
            audio.PlayOneShot(clip);

        }
    }
}
