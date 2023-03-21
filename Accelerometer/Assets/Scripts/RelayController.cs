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

    [SerializeField] string relayCode;
    [SerializeField] bool isHost;

    private void Awake()
    {
        instace = this;
    }

    public void SetRelayCode(string newRelayCode, bool newIsHost) {
        relayCode = newRelayCode;
        isHost = newIsHost;
    }

    public async Task<string> CreateRelay()
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3);

            relayCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            isHost = true;

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetHostRelayData(
                allocation.RelayServer.IpV4,
                (ushort)allocation.RelayServer.Port,
                allocation.AllocationIdBytes,
                allocation.Key,
                allocation.ConnectionData
                );

            return relayCode;
        }
        catch(RelayServiceException e)
        {
            Debug.Log(e);
            return null;
        }
    }

    public void StartHost() {
        NetworkManager.Singleton.StartHost();
    }

    public void StartHostClient() {
        Debug.Log("Soy host = " + isHost + " y mi id es: " + relayCode);
        if (isHost)
            StartHost();
        else
            JoinRelay();
    }

    public async void JoinRelay()
    {
        try
        {
            isHost = false;
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(relayCode);

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

    public void StartClient() {
        NetworkManager.Singleton.StartClient();
    }
}
