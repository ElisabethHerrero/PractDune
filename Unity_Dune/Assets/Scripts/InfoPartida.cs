using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class InfoPartida
{
    public static Guid CurrentGameId { get; set; }

   public static void ClearGameId()
    {
        CurrentGameId = Guid.Empty;
    }


}
