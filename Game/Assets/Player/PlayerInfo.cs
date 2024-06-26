using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public static class PlayerInfo
{
    private static GameObject player;
    private static playerLook pl;
    private static PlayerHealth playerHealth;
    private static Movement movement;
    private static GunShooter playerGun;
    private static Camera cam;
    private static Melee melee;
    private static ItemPool itemPool;
    private static int money = 2000;
    private static int score;

    private static Stats.conditionalDict conditionalDictTransfer = new Stats.conditionalDict();
    private static Stats.numericalDict numericalDictTransfer = new Stats.numericalDict();

    private static List<Shield> shields = new List<Shield>();

    private static GunShooter.Echelon echelon;

    public static GameObject GetPlayer(){
        return player;
    }

    public static void SetPlayer(GameObject player2){
        player = player2;
    }

    public static playerLook GetPL(){
        return pl;
    }

    public static void SetPL(playerLook pl2){
        pl = pl2;
    }

    public static PlayerHealth GetPH(){
        return playerHealth;
    }

    public static void SetPH(PlayerHealth ph){
        playerHealth = ph;
    }

    public static Movement GetMovement(){
        return movement;
    }

    public static void SetMovement(Movement movement2){
        movement = movement2;
    }

    public static GunShooter GetGun(){
        return playerGun;
    }

    public static void SetGun(GunShooter gun){
        playerGun = gun;
    }

    public static Camera GetCam(){
        return cam;
    }

    public static void SetCam(Camera cam2){
        cam = cam2;
    }

    public static Melee GetMelee(){
        return melee;
    }

    public static void SetMelee(Melee mel){
        melee = mel;
    }

    public static ItemPool GetIP(){
        return itemPool;
    }

    public static void SetIP(ItemPool ip){
        itemPool = ip;
    }

    public static int GetScore(){
        return score;
    }

    public static void SetScore(int score2){
        score = score2;
    }

    public static void AddScore(int score2){
        score += score2;
    }

    public static int GetMoney(){
        return money;
    }

    public static void SetMoney(int money2){
        money += money2;
    }

    public static void SetMoneyAbsolute(int money2){
        money = money2;
    }

    public static Stats.conditionalDict GetConditionals(){
        return conditionalDictTransfer;
    }

    public static void SetConditionals(Stats.conditionalDict conds){
        conditionalDictTransfer = conds;
    }

    public static Stats.numericalDict GetNumericals(){
        return numericalDictTransfer;
    }

    public static void SetNumericals(Stats.numericalDict conds){
        numericalDictTransfer = conds;
    }

    public static List<Shield> GetShields(){
        return shields;
    }

    public static void SetShields(List<Shield> shieldss){
        shields = shieldss;
    }

    public static GunShooter.Echelon GetEchelon(){
        return echelon;
    }

    public static void SetEchelon(int index){
        echelon = (GunShooter.Echelon)index;
    }
}
