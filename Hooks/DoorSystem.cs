using HarmonyLib;
using Unity.Collections;
using ProjectM;
using AutoCloseDoors.Systems;
using ProjectM.Gameplay.Systems;

namespace AutoCloseDoors.Hooks
{
    [HarmonyPatch(typeof(OpenDoorSystem), nameof(OpenDoorSystem.OnUpdate))]
    public class OpenDoorSystem_Patch
    {
        private static void Prefix(OpenDoorSystem __instance)
        {
            if (AutoCloseDoor.isAutoCloseDoor)
            {
                if (__instance.__OnUpdate_LambdaJob0_entityQuery != null)
                {
                    var entities = __instance.__OnUpdate_LambdaJob0_entityQuery.ToEntityArray(Allocator.Temp);
                    foreach (var entity in entities)
                    {
                        var Target = __instance.EntityManager.GetComponentData<SpellTarget>(entity).Target;
                        AutoCloseDoor.DoorReceiver(Target, __instance.EntityManager);
                    }
                }
            }
        }
    }

    [HarmonyPatch(typeof(OpenDoorsSystem), nameof(OpenDoorsSystem.OnUpdate))]
    public class OpenDoorsSystem_Patch
    {
        private static void Prefix(OpenDoorsSystem __instance)
        {
            if (AutoCloseDoor.isAutoCloseDoor)
            {
                if (__instance.__OpenDoors_entityQuery != null)
                {
                    var entities = __instance.__OpenDoors_entityQuery.ToEntityArray(Allocator.Temp);
                    foreach (var entity in entities)
                    {
                        var Buffer = __instance.EntityManager.GetBuffer<OpenDoorsBuffer>(entity);
                        for (int i = 0; i < Buffer.Length; i++)
                        {
                            var door_entity = Buffer[i].DoorEntity;
                            var Door = __instance.EntityManager.GetComponentData<Door>(door_entity);
                            Door.AgeSinceOpened = 9999999999;
                            Door.AutoCloseTime = AutoCloseDoor.AutoCloseTimer;
                            __instance.EntityManager.SetComponentData(door_entity, Door);
                        }
                    }
                }

                if (__instance.__CloseDoors_entityQuery != null)
                {
                    var entities = __instance.__CloseDoors_entityQuery.ToEntityArray(Allocator.Temp);
                    foreach (var entity in entities)
                    {
                        var Buffer = __instance.EntityManager.GetBuffer<OpenDoorsBuffer>(entity);
                        foreach (var item in Buffer)
                        {
                            var door_entity = item.DoorEntity;
                            var Door = __instance.EntityManager.GetComponentData<Door>(door_entity);
                            Door.AgeSinceOpened = 9999999999;
                            Door.AutoCloseTime = AutoCloseDoor.AutoCloseTimer;
                            __instance.EntityManager.SetComponentData(door_entity, Door);
                        }
                    }
                }
            }
        }
    }
}
