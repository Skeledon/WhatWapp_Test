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

    public Move GetLastMove()
    {
        if (MovesLogged.Count == 0)
            return null;
        return MovesLogged[MovesLogged.Count - 1];
    }

    public Move UndoMove()
    {
        Move m = GetLastMove();
        if (m == null)
            return null;
        MovesLogged.Remove(m);
        m = new Move(m.MovedCard, m.To, m.From, true, m.TableauFlipped, m.RollBack);
        return m;

    }
}
