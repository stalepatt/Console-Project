namespace meetMoltres
{
    static class Movement
    {
        // 플레이어를 이동한다.
        public static void MovePlayer(ConsoleKey key, Player player)
        {
            if (key == ConsoleKey.LeftArrow)
            {
                MoveToLeftOfTarget(out player.X, in player.X);
                player.MoveDirection = Direction.Left;
            }
            if (key == ConsoleKey.RightArrow)
            {
                MoveToRightOfTarget(out player.X, in player.X);
                player.MoveDirection = Direction.Right;
            }
            if (key == ConsoleKey.UpArrow)
            {
                MoveToUpOfTarget(out player.Y, in player.Y);
                player.MoveDirection = Direction.Up;
            }
            if (key == ConsoleKey.DownArrow)
            {
                MoveToDownOfTarget(out player.Y, in player.Y);
                player.MoveDirection = Direction.Down;
            }
        }
        // target이 있는 경우 이동 처리
        public static void MoveToLeftOfTarget(out int x, in int target) => x = Math.Max(Game.MIN_X, target - 1);
        public static void MoveToRightOfTarget(out int x, in int target) => x = Math.Min(target + 1, Game.MAX_X);
        public static void MoveToUpOfTarget(out int y, in int target) => y = Math.Max(Game.MIN_Y, target - 1);
        public static void MoveToDownOfTarget(out int y, in int target) => y = Math.Min(target + 1, Game.MAX_Y);

        public static void PushOut(Direction playerMoveDirection, ref int objX, ref int objY, int collidedObjX, int collidedObjY)
        {
            switch (playerMoveDirection)
            {
                case Direction.Left:
                    MoveToRightOfTarget(out objX, in collidedObjX);
                    break;
                case Direction.Right:
                    MoveToLeftOfTarget(out objX, in collidedObjX);
                    break;
                case Direction.Up:
                    MoveToDownOfTarget(out objY, in collidedObjY);
                    break;
                case Direction.Down:
                    MoveToUpOfTarget(out objY, in collidedObjY);
                    break;
                default:
                    Game.ExitWithError($"[Error] 플레이어의 이동 방향이 잘못되었습니다.");
                    return;
            }
        }
        public static void MoveRock(Player player, Rock rock)
        {
            switch (player.MoveDirection)
            {
                case Direction.Left:
                    {
                        MoveToLeftOfTarget(out rock.X, in player.X);
                    }
                    break;
                case Direction.Right:
                    {
                        MoveToRightOfTarget(out rock.X, in player.X);
                    }
                    break;
                case Direction.Up:
                    {
                        MoveToUpOfTarget(out rock.Y, in player.Y);
                    }
                    break;
                case Direction.Down:
                    {
                        MoveToDownOfTarget(out rock.Y, in player.Y);
                    }
                    break;
                default:
                    {
                        Game.ExitWithError($"[Error] 플레이어의 이동 방향이 잘못되었습니다.");
                    }
                    break;
            }
        }
    }
}
