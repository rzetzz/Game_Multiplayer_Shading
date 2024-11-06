using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class CharacterNetworkManager : NetworkBehaviour
{
    [Header("Position")]
    // public NetworkVariable<Vector3> networkPostition = new NetworkVariable<Vector3>(Vector3.zero,NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    // public NetworkVariable<Quaternion> networkRotation = new NetworkVariable<Quaternion>(Quaternion.identity,NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<FixedString32Bytes> playerName = new NetworkVariable<FixedString32Bytes>("User",NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public Vector3 networkVelocity;
    public float networkPositionSmoothTime = .1f;
    public float networkRotationSmoothTime = .1f;

    private void Update() {
        if (IsOwner)
        {
            // networkPostition.Value = transform.position;
            // networkRotation.Value = transform.rotation;
            playerName.Value = GetComponent<PlayerControl>().playerName;
        }
        else
        {
            // transform.position = Vector3.SmoothDamp(transform.position, networkPostition.Value,ref networkVelocity,networkPositionSmoothTime);
            // transform.rotation = Quaternion.Slerp(transform.rotation, networkRotation.Value,networkRotationSmoothTime);
            GetComponent<PlayerControl>().playerName = playerName.Value;

        }
    }
}
