using System;
using UnityEngine;


[CreateAssetMenu]
public class FacilitySetting : ScriptableObject
{
    [SerializeField] FacilityListData facilityListData;


    [NonSerialized]
    string[] facilityName = new string[] {
        "スイカ畑",
        "ビニールハウス",
        "工場",
        "魔法の庭園",
        "生命の泉",
        "収穫の神殿",
        "時の園",
        "ブラックホール工房",
        "重力の庭",
        "空間の扉",
        "星の果樹園",
        "宇宙農園",
        "星間栽培場",
        "量子収穫場",
        "超次元ゲート"
    };

    [NonSerialized]
    string[] facilityDescription = new string[] {
        "スイカの栽培をする畑",
        "一年中スイカが栽培できる畑",
        "スイカを組み立てる謎の工場",
        "魔法の力で一瞬にしてスイカを成長させる庭園",
        "自然の力が満ちる泉で、スイカが自然に成長する",
        "古代の技術を使い、大幅にスイカの生産を増やす神殿",
        "時間を操る技術で、連続してスイカを収穫する",
        "重力の異常を利用してスイカを大量に生産する工房",
        "強化された重力で、スイカの成長を促進する庭",
        "異なる空間から瞬時にスイカを運び込む扉",
        "星のエネルギーを利用して効率よくスイカを育てる果樹園",
        "宇宙の独特な環境を利用してスイカを栽培する農園",
        "広範囲に星間を渡りながらスイカを育てる栽培場",
        "量子力学の原理を用いて無数のスイカを即座に収穫する",
        "異次元の資源を活用して無限にスイカを供給するゲート"
    };
    //クリックによる収入の増加
    [NonSerialized] string[] clickName = new string[] { "理解したクリック", "手慣れたクリック", "凄腕クリック", "クリックしすぎ", "異様なクリック", "えげつないクリック", "永遠クリッカー" };

    //クリックによる収入が施設生産量に応じてパーセント増加
    [NonSerialized] string[] clickParName = new string[] { "影響タップ", "人気タップ", "神秘タップ", "パワータップ", "タップの神", "伝説のタップ", "全宇宙タップ" };

    //クリック連打によるブーストの最大値強化
    [NonSerialized] string[] clickMaxName = new string[] { "ブースト強化", "ブースト超強化", "スーパーブースト", "メガブースト", "ギガブースト", "テラブースト", "コスモブースト" };

    //施設の数による生産量上昇
    [NonSerialized] string[] facilityNumName = new string[] { "施設数補正", "数量の力", "多数の力", "大量生産", "巨大工場群", "スイカ生産帝国" };

    //施設の強化
    [NonSerialized] string[] facilityPowerName = new string[] { "強化版{0}", "改良版{0}", "効率化版{0}", "拡張版{0}", "高性能版{0}", "進化版{0}", "超高性能版{0}", "究極版{0}" };


    //生産量パーセント上昇

    public Upgrade[] UpgradeList = new Upgrade[]
     {
new Upgrade("土壌改良計画", "土壌の質を向上させ、栄養素の吸収を助けます。", 1000, 1),
new Upgrade("高度灌漑システム", "灌漑システムを高度化し、水の使用効率を大幅に改善します。", 1000, 1),
new Upgrade("自動収穫機", "収穫作業を自動化することで、労力を大きく削減します。", 1000, 1),
new Upgrade("速成育成法", "成長周期を短縮する技術を導入し、迅速に収穫へと導きます。", 5000, 1),
new Upgrade("クロスブリーディング", "異なる品種の掛け合わせによる、新しいタイプのスイカを開発します。", 5000, 1),
new Upgrade("強化肥料の開発", "栄養豊富な肥料を使用することで、スイカの成長速度と品質を向上させます。", 5000, 1),
new Upgrade("耐病性向上", "病害虫に強いスイカの開発により、より安定した収穫が可能になります。", 5000, 1),
new Upgrade("水分管理システム", "水分供給を最適化し、スイカの健康を維持します。", 25000, 5),
new Upgrade("日照コントロール技術", "適切な日照管理により、スイカの成長を促進します。", 5000000, 1),
new Upgrade("品種改良プログラム", "品種改良を通じて、より多くの収穫が期待できるスイカを育成します。", 5000000, 1),
new Upgrade("集約栽培技法", "限られた土地での効率的な栽培方法を開発し、生産量を増加させます。", 20000000, 3),
new Upgrade("生態系調和農法", "農園の生態系を整えることで、持続可能な農業を実現します。", 5e+9, 1),
new Upgrade("再生可能エネルギー活用", "再生可能エネルギーの導入により、コストを削減し環境負荷を低減します。", 5e+9, 1),
new Upgrade("精密栽培ロボティクス", "先進的なロボット技術を農園に導入し、精密な栽培が可能になります。", 5e+9, 1),
new Upgrade("持続可能な生産システム", "資源の再利用と効率的な生産システムを確立します。", 5e+9, 1),
new Upgrade("高効率光合成強化", "光合成効率を科学的に強化し、スイカの生育を促進します。", 5e+11, 2),
new Upgrade("スマート農園管理", "データ駆動型の農園管理により、全てのプロセスを最適化します。", 5e+11, 2),
new Upgrade("次元間育成技法", "地球外環境でも生育可能なスイカを開発し、新たな農地を拓く準備をします。", 5e+11, 2),
new Upgrade("宇宙適応育成キット", "宇宙適応型栽培技術を用いて、地球外でもスイカを育てることができます。", 5e+11, 2),
new Upgrade("時間制御栽培法", "時間制御技術を用いて、生育サイクルを自由に操ります。", 5e+11, 2),
new Upgrade("量子光合成加速", "量子技術を用いて光合成プロセスを加速し、収穫速度を増加させます。", 5e+15, 3),
new Upgrade("星間育成モジュール", "星間距離を超えた効率的な輸送技術で、新たな市場に挑みます。", 5e+15, 3),
new Upgrade("多次元育成環境", "複数の次元を活用した栽培技術で、未知の可能性に挑戦します。", 5e+15, 3),
new Upgrade("銀河横断育成技術", "銀河系全域にわたる農園を管理し、異なる環境での栽培を実現します。", 5e+15, 3),
new Upgrade("超光速輸送システム", "高速でのスイカ輸送を実現し、新鮮なままで広範囲に供給します。", 5e+15, 3),
new Upgrade("宇宙規模生産基地", "大規模な宇宙農園を設立し、宇宙開発の一翼を担います。", 1e+20, 5),
new Upgrade("星間交易ネットワーク", "星間での貿易を拡大し、スイカ市場のグローバル化を推進します。", 1e+21, 5),
new Upgrade("宇宙連合農場", "宇宙連合に参加し、多種多様な生態系でスイカを栽培します。", 1e+22, 5),
new Upgrade("量子界層育成プラットフォーム", "量子技術を活用し、スイカの生産を革新的な方法で拡張します。", 1e+23, 5),
new Upgrade("無限次元収穫ネットワーク", "無限の可能性を持つ次元を開拓し、スイカ生産の新時代を切り開きます。", 1e+24, 5)
     };





    [ContextMenu("施設のコスト")]
    private void PrivateMethod()
    {
        for (int i = 0; i < facilityListData.facilityItemList.Count && i < facilityName.Length; i++)
        {
            var facility = facilityListData.facilityItemList[i];
            facility.id = i;
            facility.baseCost = Math.Floor(50 * facility.basePower * Math.Pow(1.5, i));
            facility.name = facilityName[i];
            facility.description = facilityDescription[i];
        }
    }
    [ContextMenu("強化の指定")]
    private void PrivateMethod2()
    {
        //100～199
        int baseIndex = 100;
        double[] clickNums = new double[] { 80, 2500, 5000, 15000, 200000, 500000, 2500000 };
        int[] clickPower = new int[] { 2, 3, 4, 5, 5, 5, 10 };
        for (int i = 0; i < clickNums.Length; i++)
        {
            var index = i + baseIndex;
            var item = facilityListData.powerUpItemList.Find(a => { return a.editNum == index; });
            if (item == null)
            {
                item = new PowerUpItem();
                facilityListData.powerUpItemList.Add(item);
                item.editNum = index;
            }
            item.idIndex = i;

            item.id = "click";
            item.cost = Math.Floor(100 * Math.Pow(500, item.idIndex) * (1 + 0.25 * item.idIndex));
            item.kind = PowerUpKind.Click;
            item.lockKind = PowerUpKind.Click;
            item.lockNum = clickNums[i];
            item.name = clickName[i];
            item.power = clickPower[i];
            item.manual = string.Format("クリックが{0}倍になる", "{0}");
        }



        //300～399
        baseIndex = 200;
        int[] clickParNums = new int[] { 1000, 10000, 50000, 200000, 1000000, 2000000, 7500000 };
        for (int i = 0; i < clickNums.Length; i++)
        {
            var index = i + baseIndex;
            var item = facilityListData.powerUpItemList.Find(a => { return a.editNum == index; });
            if (item == null)
            {
                item = new PowerUpItem();
                facilityListData.powerUpItemList.Add(item);
                item.editNum = index;
            }
            item.idIndex = i;
            item.id = "click_par";
            item.cost = Math.Floor(100 * Math.Pow(500, item.idIndex) * (1 + 0.55 * item.idIndex));
            item.kind = PowerUpKind.ClickPar;
            item.lockKind = PowerUpKind.Click;
            item.lockNum = clickParNums[i];
            item.name = clickParName[i];
            item.power = 1;
            item.manual = string.Format("クリックが収入の{0}%増加", "{0}");
        }
        //300～399
        baseIndex = 300;
        int[] clickMaxNums = new int[] { 100, 1000, 10000, 25000, 500000, 1000000, 5000000 };
        for (int i = 0; i < clickMaxName.Length; i++)
        {
            var index = i + baseIndex;
            var item = facilityListData.powerUpItemList.Find(a => { return a.editNum == index; });
            if (item == null)
            {
                item = new PowerUpItem();
                facilityListData.powerUpItemList.Add(item);
                item.editNum = index;
            }
            item.idIndex = i;
            item.id = "click_max";
            item.cost = Math.Floor(200 * Math.Pow(500, item.idIndex) * (1 + 0.85 * item.idIndex));
            item.kind = PowerUpKind.ClickBoost;
            item.lockKind = PowerUpKind.Click;
            item.lockNum = clickMaxNums[i];
            item.name = clickMaxName[i];
            item.power = 50;
            item.manual = string.Format("最大クリックブーストが{0}増加", "{0}");
        }


        //400～499
        baseIndex = 400;
        for (int i = 0; i < facilityNumName.Length; i++)
        {
            var index = i + baseIndex;
            var item = facilityListData.powerUpItemList.Find(a => { return a.editNum == index; });
            if (item == null)
            {
                item = new PowerUpItem();
                facilityListData.powerUpItemList.Add(item);
                item.editNum = index;
            }
            item.idIndex = i;
            item.id = "facility_num";
            item.cost = Math.Floor(10000 * Math.Pow(2000, item.idIndex) * (1 + 0.85 * item.idIndex));
            item.kind = PowerUpKind.FacilityNum;
            item.lockKind = PowerUpKind.ALL;
            item.lockNum = item.cost;
            item.name = facilityNumName[i];
            item.power = 2;
            item.manual = string.Format("施設1つにつき、収入+0.0{0}%", "{0}");
        }

        baseIndex = 1000;
        int[] facilityLockNum = new int[] { 5, 25, 50, 100, 200, 500, 750, 1000 };
        double[] facilityCostMulti = new double[] { 10, 50, 500, 5000, 500000, 2500000, 500000000, 5000000000 };
        int[] facilityPower = new int[] { 2, 3, 4, 5, 5, 5, 10, 20 };
        for (int j = 0; j < facilityListData.facilityItemList.Count; j++)
        {
            int baseNum = j * 20;
            for (int i = 0; i < facilityPowerName.Length; i++)
            {
                var index = i + baseIndex + baseNum;
                PowerUpItem item = facilityListData.powerUpItemList.Find(a => { return a.editNum == index; });
                if (item == null)
                {
                    //Debug.LogError(string.Format("[{0}]が無い", i));
                    //continue;
                    item = new PowerUpItem();
                    facilityListData.powerUpItemList.Add(item);
                    item.editNum = index;
                }
                item.id = "facilityPower_" + j;
                item.name = string.Format(facilityPowerName[i], facilityListData.facilityItemList[j].name);
                item.power = facilityPower[i];
                item.powerId = j;
                item.idIndex = i;
                item.kind = PowerUpKind.Facility;

                item.lockKind = PowerUpKind.Facility;
                item.lockId = j;
                item.lockNum = facilityLockNum[i];
                item.manual = string.Format("{0}が{1}倍になる", facilityListData.facilityItemList[j].name, "{0}");
                item.cost = Math.Floor(facilityListData.facilityItemList[j].baseCost * facilityCostMulti[i]);
            }
        }

        baseIndex = 3000;
        for (int i = 0; i < UpgradeList.Length; i++)
        {
            var index = i + baseIndex;
            var item = facilityListData.powerUpItemList.Find(a => { return a.editNum == index; });
            if (item == null)
            {
                Debug.LogError(string.Format("[{0}]が無い", i));
                item = new PowerUpItem();
                facilityListData.powerUpItemList.Add(item);
                item.editNum = index;
            }
            item.SetSetting(UpgradeList[i]);
            item.idIndex = i;
            item.kind = PowerUpKind.ALL;
            item.lockKind = PowerUpKind.ALL;
            item.manual = string.Format("収入が+{0}%増加する", "{0}");
        }
        facilityListData.powerUpItemList.Sort((a, b) => (a.editNum - b.editNum));
    }
}


[Serializable]
public struct Upgrade
{
    public string Name;
    public string Description;
    public double Cost;
    public int Effect;

    public Upgrade(string name, string description, double cost, int effect)
    {
        Name = name;
        Description = description;
        Cost = cost;
        Effect = effect;
    }
}