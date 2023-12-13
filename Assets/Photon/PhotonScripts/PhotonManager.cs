using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace Youchan
{
    public class PhotonManager : MonoBehaviourPunCallbacks
    {
        private string gameVersion = "2";
        public string userId = "Youchan";
        void Awake()
        {
            // 같은 방의 유저들에게 자동으로 씬을 로딩
            PhotonNetwork.AutomaticallySyncScene = true;
            //같은 버전의 유저끼리 접속 허용
            PhotonNetwork.GameVersion = gameVersion;
            //유저 아이디 할당
            PhotonNetwork.NickName = userId;
            //포톤 서버와 통신 횟수 설정(30프레임)
            Debug.Log(PhotonNetwork.SendRate);
            //서버 접속
            PhotonNetwork.ConnectUsingSettings();
        }

        //포톤 서버에 접속 후 호출되는 콜백 함수
        public override void OnConnectedToMaster()
        {          
            Debug.Log("Connect Master");
            Debug.Log($"PhotonNetwork.InLobby = {PhotonNetwork.InLobby}"); //$ => String.Format() 문자열로 반환
            PhotonNetwork.JoinLobby(); // 로비에 접속
        }
        //로비 접속 후 호출되는 콜백 함수
        public override void OnJoinedLobby()
        {
            Debug.Log($"PhotonNetwork.InLobby = {PhotonNetwork.InLobby}");
            PhotonNetwork.JoinRandomRoom(); //랜덤 매치메이킹 기능
        }
        //랜덤 룸에 못들어갔을 때 호출되는 콜백 함수
        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log($"JoinRandomFailed {returnCode} : {message}");

            //룸의 속성 정의
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 20; //최대 접속자 수 (CCU)
            roomOptions.IsOpen = true; //룸의 오픈 여부
            roomOptions.IsVisible = true; //로비에서 룸 목록에 나타낼지의 여부

            //룸 생성
            PhotonNetwork.CreateRoom("My room", roomOptions);
        }

        //룸 생성이 완료된 후 호출되는 콜백 함수
        public override void OnCreatedRoom()
        {
            Debug.Log("Created Room");
            Debug.Log($"Room Name = {PhotonNetwork.CurrentRoom.Name}");
        }

        //룸에 입장한 후 호출되는 콜백 함수
        public override void OnJoinedRoom()
        {
            Debug.Log($"PhotonNetwork.InRoom = {PhotonNetwork.InRoom}");
            Debug.Log($"PlayerCount = {PhotonNetwork.CurrentRoom.PlayerCount}");

            //룸에 접속한 사용자 정보 확인
            foreach(var player in PhotonNetwork.CurrentRoom.Players)
            {
                Debug.Log($"{player.Value.NickName}, {player.Value.ActorNumber}");//ActorNumber = player의 고유번호
            }
            //캐릭터 출현 정보를 배열에 저장
            Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
            int index = Random.Range(0, points.Length-1);
            //캐릭터 생성
            PhotonNetwork.Instantiate("Player1", points[index].position, points[index].rotation, 0);
        }

    }
}
