using System.Collections.Generic;

public interface IMoveWithDirection: IMovable, IDirection{
    public bool CanMoveWithDirection(IGround ground);
    public List<MoveType> GetPossibleMoves();
}