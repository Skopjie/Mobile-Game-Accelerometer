using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using System.Threading.Tasks;

public class RelayController : MonoBehaviour
{

    public static RelayController Instance { get { return instace; } }
    private static RelayController instace;

    [Header("Componentes")]
    public LobbyPlayerRelayData relayData;

    private void Awake()
    {
        instace = this;
    }

    public void SetRelayData(string newCodeRelay, string newNamePlayer, int newNumberPlayer, bool newIsHost) {
        relayData.codeRelay = newCodeRelay;
        relayData.namePlayer = newNamePlayer;
        relayData.numberPlayer = newNumberPlayer;
        relayData.isHost = newIsHost;
    }

    //creamos una partida 
    public async Task<string> CreateRelay(string newNamePlayer, int newNumberPlayer)
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(4);
            relayData.allocation = allocation; 
            string newRelayCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            SetRelayData(newRelayCode, newNamePlayer, newNumberPlayer, true);

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetHostRelayData(
                relayData.allocation.RelayServer.IpV4,
                (ushort)relayData.allocation.RelayServer.Port,
                relayData.allocation.AllocationIdBytes,
                relayData.allocation.Key,
                relayData.allocation.ConnectionData
                );

            NetworkManager.Singleton.StartHost();

            return relayData.codeRelay;
        }
        catch(RelayServiceException e)
        {
            Debug.Log(e);
            return null;
        }
    }


    public async void JoinRelay()
    {
        try
        {
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(relayData.codeRelay);

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData(
               joinAllocation.RelayServer.IpV4,
               (ushort)joinAllocation.RelayServer.Port,
               joinAllocation.AllocationIdBytes,
               joinAllocation.Key,
               joinAllocation.ConnectionData,
               joinAllocation.HostConnectionData
               );

            NetworkManager.Singleton.StartClient();
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }
    }
}
