using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLog
{
    private List<Move> MovesLogged = new List<Move>();

    public void LogMove(Move m)
    {
        MovesLogged.Add(m);
    }
}
