namespace JohaToolkit.Unity.DataStructures.Graph
{
    public class Vertex
    {
        public readonly string Identifier;
        public readonly float PosX;
        public readonly float PosY;
        public readonly float PosZ;

        public Vertex(string identifier, float posX, float posY, float posZ)
        {
            Identifier = identifier;
            PosX = posX;
            PosY = posY;
            PosZ = posZ;
        }
    }
}
