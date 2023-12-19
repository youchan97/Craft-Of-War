using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelineController : MonoBehaviour
{
    public TimelineAsset timeline;
    public TrackAsset[] track = new TrackAsset[3];
    public PlayableDirector playable;
    void Awake()
    {
        playable = GetComponent<PlayableDirector>();
        for (int i = 0; i < track.Length; i++)
        {
            track[i] = timeline.GetRootTrack(i);
            track[i].muted = true;
        }
    }


    //보람누나 테스트 코드 없어도됌
    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Z))
    //    {
    //        track[0] = timeline.GetRootTrack(0);
    //        track[0].muted = false;
    //        track[1] = timeline.GetRootTrack(1);
    //        track[1].muted = true;
    //        track[2] = timeline.GetRootTrack(2);
    //        track[2].muted = true;
    //        playable.Play();
    //    }

    //    if (Input.GetKeyDown(KeyCode.X))
    //    {
    //        track[0] = timeline.GetRootTrack(0);
    //        track[0].muted = true;
    //        track[1] = timeline.GetRootTrack(1);
    //        track[1].muted = false;
    //        track[2] = timeline.GetRootTrack(2);
    //        track[2].muted = true;
    //        playable.Play();
    //    }

    //    if (Input.GetKeyDown(KeyCode.C))
    //    {
    //        track[0] = timeline.GetRootTrack(0);
    //        track[0].muted = true;
    //        track[1] = timeline.GetRootTrack(1);
    //        track[1].muted = true;
    //        track[2] = timeline.GetRootTrack(2);
    //        track[2].muted = false;
    //        playable.Play();
    //    }
    //}
}
