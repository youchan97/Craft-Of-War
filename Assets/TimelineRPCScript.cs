using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Playables;

public class TimelineRPCScript : MonoBehaviour
{
    public PlayableDirector pd;
    [PunRPC]
    public void StartTimeline()
    {
        pd.enabled = true;
    }

}
