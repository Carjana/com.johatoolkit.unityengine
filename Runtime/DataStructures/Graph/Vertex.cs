using System;

namespace JohaToolkit.UnityEngine.DataStructures.Graph
{
    [Obsolete]
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
