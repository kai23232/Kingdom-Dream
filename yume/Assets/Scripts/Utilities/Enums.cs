
using System;

[Flags]
public enum RoomType
{
    // 普通敌人房间（基础怪物战斗场景）
    MinorEnemy = 1,
    // 精英敌人房间（高难度怪物战斗，掉落更优奖励）
    EliteEnemy = 2,
    // 商店房间（可消耗货币购买道具/装备）
    Shop = 4,
    // 宝藏房间（无战斗，包含宝箱/资源奖励）
    Treasure = 8,
    // 休息房间（恢复血量/状态，可解锁临时增益）
    RestRoom = 16,
    // 首领房间（最终/阶段性BOSS战，核心奖励获取点）
    Boss = 32
}

// 房间状态枚举（用于标记房间在关卡流程中的状态）
public enum RoomState
{
    // 已上锁（无法进入该房间）
    Locked,
    // 已访问（玩家已进入并完成房间内交互/战斗）
    Visited,
    // 可到达（房间已解锁，玩家可移动至该房间）
    Attainable,
}