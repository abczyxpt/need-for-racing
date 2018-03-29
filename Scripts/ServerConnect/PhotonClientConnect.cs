using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonClientConnect{

    private static PhotonClientConnect instance = null;

    public static PhotonClientConnect Get
    {
        get { return instance ?? (instance = new PhotonClientConnect()); }
    }
    
}
