using ProjectM;
using Unity.Collections;
using Unity.Entities;

namespace AutoCloseDoors.Systems
{
    public static class AutoCloseDoor
    {
        public static bool isAutoCloseDoor = true;
        public static float AutoCloseTimer = 2.0f;
        public static bool isEnableUninstall = false;
        public static FixedString512 UninstallCommand = "~autoclosedooruninstall";
        public static EntityManager em = Plugin.Server.EntityManager;

        public static void DoorReceiver(Entity entity, EntityManager em)
        {
            if (em.HasComponent<Door>(entity))
            {
                var Door = em.GetComponentData<Door>(entity);
                Door.AutoCloseTime = AutoCloseTimer;
                em.SetComponentData(entity, Door);
            }
        }

        public static void RevertAutoClose()
        {
            var DoorQuery = em.CreateEntityQuery(new EntityQueryDesc()
            {
                All = new ComponentType[] {
                            ComponentType.ReadOnly<Door>(),
                        },
                Options = EntityQueryOptions.IncludeDisabled
            });

            var DoorEntities = DoorQuery.ToEntityArray(Allocator.Temp);
            if (DoorEntities.Length <= 0)
            {
                Plugin.Logger.LogError($"There are no doors in the server!!");
                return;
            }
            foreach (var entity in DoorEntities)
            {
                var Door = em.GetComponentData<Door>(entity);
                Door.AutoCloseTime = 0;
                em.SetComponentData(entity, Door);
            }
            Plugin.Logger.LogWarning($"All doors (Total: {DoorEntities.Length}) has been reverted back to normal.");
        }

        public static void InitializeAutoClose()
        {
            var DoorQuery = em.CreateEntityQuery(new EntityQueryDesc()
            {
                All = new ComponentType[] {
                            ComponentType.ReadOnly<Door>(),
                        },
                Options = EntityQueryOptions.IncludeDisabled
            });

            var DoorEntities = DoorQuery.ToEntityArray(Allocator.Temp);
            if (DoorEntities.Length <= 0)
            {
                Plugin.Logger.LogError($"There are no doors in the server!!");
                return;
            }
            foreach (var entity in DoorEntities)
            {
                var Door = em.GetComponentData<Door>(entity);
                Door.AutoCloseTime = AutoCloseTimer;
                em.SetComponentData(entity, Door);
            }
            Plugin.Logger.LogWarning($"All doors (Total: {DoorEntities.Length}) has been set to auto close at {AutoCloseTimer}s.");
        }
    }
}
