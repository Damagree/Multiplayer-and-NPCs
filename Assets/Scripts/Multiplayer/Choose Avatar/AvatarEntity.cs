using UnityEngine;

[CreateAssetMenu]
public class AvatarEntity : ScriptableObject
{
    public string avatarUrl = "";
    [TextArea(3, 5)] public string jsonAvatar = "";
}
