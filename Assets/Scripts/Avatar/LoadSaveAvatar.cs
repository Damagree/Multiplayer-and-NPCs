using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Linq;
using UnityEngine;

public class LoadSaveAvatar : MonoBehaviour
{
    public static  AvatarEntity avatarEntity;

    public static void Save(GameObject avatar) {
        //avatar = avatarEntity.avatar;

        string jsonString = JsonUtility.ToJson(avatar);
    }

}
