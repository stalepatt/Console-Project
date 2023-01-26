namespace meetMoltres
{
    public static class CollisionHelper
    {
        // 충돌 처리
        public static void OnCollision(Action action)
        {
            action();
        }

        // 충돌 판정        
        public static bool IsCollided(int x1, int x2, int y1, int y2)
        {
            if (x1 == x2 && y1 == y2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
