using System.Collections.Generic;

namespace WorldForging
{
    public sealed class WorldForger
    {
        public WorldForgerContext Forge(WorldForgerRequest request)
        {
            var context = new WorldForgerContext(request.ChunkPositions.Count);
            var steps = CreateSteps(context, request);
        
            foreach (var step in steps)
            {
                step.Forge();
            }

            return context;
        }

        private List<IWorldForgerStep> CreateSteps(WorldForgerContext context, WorldForgerRequest request)
        {
            return new()
            {
                new CreateBaseTerrainStep(context, request)
            };
        }
    }
}