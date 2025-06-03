namespace ME.BECS.FixedPoint {

    //
    // FixPointCS
    //
    // Copyright(c) Jere Sanisalo, Petri Kero
    //
    // Permission is hereby granted, free of charge, to any person obtaining a copy
    // of this software and associated documentation files (the "Software"), to deal
    // in the Software without restriction, including without limitation the rights
    // to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    // copies of the Software, and to permit persons to whom the Software is
    // furnished to do so, subject to the following conditions:
    //
    // The above copyright notice and this permission notice shall be included in all
    // copies or substantial portions of the Software.
    //
    // THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    // IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    // FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    // AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    // LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    // OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    // SOFTWARE.
    //

    using System.Diagnostics;
    using System.Globalization;
    using System.Runtime.CompilerServices;

    #if FIXED_POINT_F32
    public static class FixedUtil {

        public static void InvalidArgument(string funcName, string argName, int argValue) {
            UnityEngine.Debug.LogError($"{funcName}, {argName}, {argValue}");
        }

        public static void InvalidArgument(string funcName, string argNames, int argValue1, int argValue2) {
            UnityEngine.Debug.LogError($"{funcName}, {argNames}, {argValue1}, {argValue2}");
        }

        public static void InvalidArgument(string funcName, string argName, long argValue) {
            UnityEngine.Debug.LogError($"{funcName}, {argName}, {argValue}");
        }

        public static void InvalidArgument(string funcName, string argNames, long argValue1, long argValue2) {
            UnityEngine.Debug.LogError($"{funcName}, {argNames}, {argValue1}, {argValue2}");
        }

        [MethodImpl(256)]
        public static int Qmul29(int a, int b) {
            return (int)(((long)a * (long)b) >> 29);
        }

        [MethodImpl(256)]
        public static int Qmul30(int a, int b) {
            return (int)(((long)a * (long)b) >> 30);
        }

        [MethodImpl(256)]
        public static int ShiftLeft(int v, int shift) {
            return shift >= 0 ? v << shift : v >> -shift;
        }

        [MethodImpl(256)]
        public static int ShiftRight(int v, int shift) {
            return shift >= 0 ? v >> shift : v << -shift;
        }

        [MethodImpl(256)]
        public static long ShiftRight(long v, int shift) {
            return shift >= 0 ? v >> shift : v << -shift;
        }

        [MethodImpl(256)]
        public static long LogicalShiftRight(long v, int shift) {
            #if JAVA
            return v >>> shift;
            #else
            return (long)((ulong)v >> shift);
            #endif
        }

        [MethodImpl(256)]
        public static int Exp2Poly3(int a) {
            var y = Qmul30(a, 84039593); // 0.0782679701835315868647357253725971674790033117245148781445598202137415363194904317749528903660739148499430948967629357887
            y = Qmul30(a, y + 242996024); // 0.226307682289372255347421644246257966273699535419878898050811760122384683941875786929647503217974831952486347597791720611
            y = Qmul30(a, y + 746706207); // 0.695424347527096157787842630381144866247297152855606223804628419663873779738633781295399606415951253197570557505445343601
            y = y + 1073741824;
            return y;
        }

        // Precision: 18.19 bits
        [MethodImpl(256)]
        public static int Exp2Poly4(int a) {
            var y = Qmul30(a, 14555373); // 0.0135557472348149177040307931905578544538124307723745221579881209426474911809748672636364432116420009120178935332926148611
            y = Qmul30(a, y + 55869331); // 0.0520323690084328924674487312215472415900450170687696511359785661622616863440911035364584944748959228308174520922142865995
            y = Qmul30(a, y + 259179547); // 0.241379762937091639018661074809242143033914474070411268883151785382581196588939527676019951096295687987322799846420118765
            y = Qmul30(a, y + 744137573); // 0.693032120819660550809859400778652760922228078088444557822881527512509625885994501523885111217166388269841854528072979774
            y = y + 1073741824;
            return y;
        }

        // Precision: 23.37 bits
        [MethodImpl(256)]
        public static int Exp2Poly5(int a) {
            var y = Qmul30(a, 2017903); // 0.00187931864849444079178064366523643962831734445578833344828943266930262096728457318136293441770024748382988959051143223706
            y = Qmul30(a, y + 9654007); // 0.0089909950956369787948425038952611903126353369666002380364841111113291819538448433335270460143993823536893996134420311419
            y = Qmul30(a, y + 59934847); // 0.0558186759615980431203104787377782200574008231751237416643519892213181468390965300572726745159591410385167709971364406724
            y = Qmul30(a, y + 257869054); // 0.240159271464382269128561549965297376067896183861587452990582800168980375711748974675500420263841816476095364031528449763
            y = Qmul30(a, y + 744266012); // 0.693151738829888268164504823736426773933750311540900233860291666829069674528025078752336924788099412647868575767381646186
            y = y + 1073741824;
            return y;
        }

        // Rcp()

        // Precision: 11.33 bits
        [MethodImpl(256)]
        public static int RcpPoly4(int a) {
            var y = Qmul30(a, 166123244); // 0.154714327545457094588979713106287560782537959277436051827019427328357322113481152734370734443893548182187731839301875899
            y = Qmul30(a, y + -581431354); // -0.54150014640909983106142899587200646273888285747102618139456799564925062739718403457029757055362741863765712410515426083
            y = Qmul30(a, y + 939345296); // 0.874833479742433164394762329205339796072216190804359514727901328982583960730517367903630903886960751970990489874952162084
            y = Qmul30(a, y + -1060908097); // -0.988047660878790427922313046439620894115871292610769385160352760661690655446814486067704067777226881515521097609099777152
            y = y + 1073741824;
            return y;
        }

        // Precision: 16.53 bits
        [MethodImpl(256)]
        public static int RcpPoly6(int a) {
            var y = Qmul30(a, 77852993); // 0.0725062501842326696626758301282171253618850679805450684783331254738896577827939599454470990870969993306249485759929666981
            y = Qmul30(a, y + -350338469); // -0.326278125829047013482041235576977064128482805912452808152499064632503460022572819754511945891936496987812268591968349959
            y = Qmul30(a, y + 723231606); // 0.673561921455982545223734340382301840739167811454662043292934558465813280438563313561682188125190962075365760110255605888
            y = Qmul30(a, y + -974250754); // -0.907341721411285515029553588773713935349385980480415781958608445152553325879092398556945676696228380651281188866035287881
            y = Qmul30(a, y + 1059679220); // 0.986903179099804504543521516658287329916121575881841375617775839327272856972646872081214163224722216327427202183393238047
            y = Qmul30(a, y + -1073045505); // -0.999351503499687190918336862818115296539305668924179897277936013481919009292338927276885827848845300094324453411638172793
            y = y + 1073741824;
            return y;
        }

        private static readonly int[] RcpPoly3Lut4Table = {
            -678697788, 1018046684, -1071069948, 1073721112,
            -302893157, 757232894, -1008066289, 1068408287,
            -154903745, 542163110, -902798392, 1051046118,
            -87262610, 392681750, -792180891, 1023631333,
        };

        // Precision: 15.66 bits
        [MethodImpl(256)]
        public static int RcpPoly3Lut4(int a) {
            var offset = (a >> 28) * 4;
            var y = Qmul30(a, RcpPoly3Lut4Table[offset + 0]);
            y = Qmul30(a, y + RcpPoly3Lut4Table[offset + 1]);
            y = Qmul30(a, y + RcpPoly3Lut4Table[offset + 2]);
            y = y + RcpPoly3Lut4Table[offset + 3];
            return y;
        }

        private static readonly int[] RcpPoly4Lut8Table = {
            796773553, -1045765287, 1072588028, -1073726795, 1073741824,
            456453183, -884378041, 1042385791, -1071088216, 1073651788,
            276544830, -708646126, 977216564, -1060211779, 1072962711,
            175386455, -559044324, 893798171, -1039424537, 1071009496,
            115547530, -440524957, 805500803, -1010097984, 1067345574,
            78614874, -348853503, 720007233, -974591889, 1061804940,
            54982413, -278348465, 641021491, -935211003, 1054431901,
            39383664, -223994590, 569927473, -893840914, 1045395281,
        };

        // Precision: 24.07 bits
        [MethodImpl(256)]
        public static int RcpPoly4Lut8(int a) {
            var offset = (a >> 27) * 5;
            var y = Qmul30(a, RcpPoly4Lut8Table[offset + 0]);
            y = Qmul30(a, y + RcpPoly4Lut8Table[offset + 1]);
            y = Qmul30(a, y + RcpPoly4Lut8Table[offset + 2]);
            y = Qmul30(a, y + RcpPoly4Lut8Table[offset + 3]);
            y = y + RcpPoly4Lut8Table[offset + 4];
            return y;
        }

        // Sqrt()

        // Precision: 13.36 bits
        [MethodImpl(256)]
        public static int SqrtPoly3(int a) {
            var y = Qmul30(a, 26809804); // 0.0249685755493961204934845015323729712245958715357182065425848552518546416164312449413742280712638308483065114885417147904
            y = Qmul30(a, y + -116435772); // -0.108439263715492087333244576730247754908569708153374339944951137491994192013534152641012071161185446185655458733810736431
            y = Qmul30(a, y + 534384395); // 0.497684250539191015641448799407572862253645711994604206579046020230872028859209946550025377417563188072362793476181318665
            y = y + 1073741824; // 1.0
            return y;
        }

        // Precision: 16.50 bits
        [MethodImpl(256)]
        public static int SqrtPoly4(int a) {
            var y = Qmul30(a, -11559524); // -0.0107656468280005064933278905326776959702034851444407595549875999349858889266381514341825269487372902092181743561671344361
            y = Qmul30(a, y + 49235626); // 0.0458542501550120083313075597659725264999808459122954966477604412728019257521420334516113399358029950852981420572751187192
            y = Qmul30(a, y + -129356986); // -0.120473082434524586846319215086079446047783981719931015048359535322204769538816469963000080874223090148906445903268633479
            y = Qmul30(a, y + 536439312); // 0.499598041480608133810028270062482694087678496329024351132266431975121211175419626795958802214798958007840324433072946221
            y = y + 1073741824; // 1.0
            return y;
        }

        private static readonly int[] SqrtPoly3Lut8Table = {
            57835763, -133550637, 536857054, 1073741824,
            43771091, -128445855, 536217068, 1073769530,
            34067722, -121273511, 534434402, 1073918540,
            27129178, -113536005, 531547139, 1074279077,
            22019236, -105917226, 527752485, 1074910452,
            18161894, -98716852, 523266057, 1075843557,
            15188335, -92049348, 518277843, 1077088717,
            12854281, -85939307, 512942507, 1078642770,
        };

        // Precision: 23.56 bits
        [MethodImpl(256)]
        public static int SqrtPoly3Lut8(int a) {
            var offset = (a >> 27) * 4;
            var y = Qmul30(a, SqrtPoly3Lut8Table[offset + 0]);
            y = Qmul30(a, y + SqrtPoly3Lut8Table[offset + 1]);
            y = Qmul30(a, y + SqrtPoly3Lut8Table[offset + 2]);
            y = y + SqrtPoly3Lut8Table[offset + 3];
            return y;
        }

        // RSqrt()

        // Precision: 10.55 bits
        [MethodImpl(256)]
        public static int RSqrtPoly3(int a) {
            var y = Qmul30(a, -91950555); // -0.0856356289309618075724442347978716997984112060739604608172096078728382955692378474864988406402256175535135909431476122756
            y = Qmul30(a, y + 299398639); // 0.278836710932968623313626076681628936089988230155462820435332822241435754263689225928134347217644388668377307808008581711
            y = Qmul30(a, y + -521939780); // -0.486094300815459291340337479778908197006741086393028323029783345373231219463397859016441739413597984747356793749404820923
            y = y + 1073741824; // 1.0
            return y;
        }

        // Precision: 16.08 bits
        [MethodImpl(256)]
        public static int RSqrtPoly5(int a) {
            var y = Qmul30(a, -34036183); // -0.0316986662178132948125724057457789067274319219669948992806572724657733410288354401675056668794389506376695226173434879395
            y = Qmul30(a, y + 140361627); // 0.130721952132469025002475913996909202114937889568059538633961150597311078891698356228999013320515864372663894767082977274
            y = Qmul30(a, y + -276049470); // -0.257091104134768572313864227992129218223743238765168155465701243532365537275047394354439662434976402678179717893043406024
            y = Qmul30(a, y + 391366758); // 0.364488696521754181874287383887070965401384329257252476170774837632778316357675818762347533724407215505228711973415321601
            y = Qmul30(a, y + -536134428); // -0.4993140971150938153494823020412230032803111204046749234700376032365842777144378210442074505666869401945364431146552564
            y = y + 1073741824; // 1.0
            return y;
        }

        private static readonly int[] RSqrtPoly3Lut16Table = {
            -301579590, 401404709, -536857690, 1073741824,
            -245423010, 391086820, -536203235, 1073727515,
            -202026137, 374967334, -534189977, 1073642965,
            -168017146, 355951863, -530632261, 1073420226,
            -141028602, 335796841, -525604155, 1073001192,
            -119367482, 315555573, -519290609, 1072343850,
            -101802870, 295846496, -511911750, 1071422108,
            -87426328, 277017299, -503685655, 1070223323,
            -75558212, 259246781, -494811415, 1068745317,
            -65683680, 242608795, -485462769, 1066993613,
            -57408255, 227112748, -475787122, 1064979109,
            -50426484, 212729399, -465907121, 1062716254,
            -44499541, 199407328, -455923331, 1060221646,
            -39439007, 187083448, -445917204, 1057513002,
            -35094980, 175689646, -435953979, 1054608400,
            -31347269, 165156947, -426085312, 1051525761,
        };

        // Precision: 24.59 bits
        [MethodImpl(256)]
        public static int RSqrtPoly3Lut16(int a) {
            var offset = (a >> 26) * 4;
            var y = Qmul30(a, RSqrtPoly3Lut16Table[offset + 0]);
            y = Qmul30(a, y + RSqrtPoly3Lut16Table[offset + 1]);
            y = Qmul30(a, y + RSqrtPoly3Lut16Table[offset + 2]);
            y = y + RSqrtPoly3Lut16Table[offset + 3];
            return y;
        }

        // Log()

        // Precision: 12.18 bits
        [MethodImpl(256)]
        public static int LogPoly5(int a) {
            var y = Qmul30(a, 34835446); // 0.0324430374324099257645920506145091908173169505782530351933872568452187970039716570286755191899094832608276898590172296967
            y = Qmul30(a, y + -149023176); // -0.138788648453891138663259214948877985710758551758834443319382469349215457727435900740974302256302169487791331019735819359
            y = Qmul30(a, y + 315630515); // 0.293953823490661881198275484636301174125635657531784266710660462843103551518793294127904893642006610982580864921150987405
            y = Qmul30(a, y + -530763208); // -0.494311758014893283441267083938639942320000878038159975670627731298854764041757060977673894877216298188619864197654458825
            y = Qmul30(a, y + 1073581542); // 0.999850726105657924558890885094884131163156956047212371206642490453141495216122729917931111298021060974997472559962156274
            return y;
        }

        private static readonly int[] LogPoly3Lut4Table = {
            270509931, -528507852, 1073614348, 0,
            139305305, -442070189, 1053671695, 1633382,
            83615845, -360802306, 1013781196, 8222843,
            52639154, -291267388, 961502851, 21386502,
        };

        // Precision: 12.51 bits
        [MethodImpl(256)]
        public static int LogPoly3Lut4(int a) {
            var offset = (a >> 28) * 4;
            var y = Qmul30(a, LogPoly3Lut4Table[offset + 0]);
            y = Qmul30(a, y + LogPoly3Lut4Table[offset + 1]);
            y = Qmul30(a, y + LogPoly3Lut4Table[offset + 2]);
            y = y + LogPoly3Lut4Table[offset + 3];
            return y;
        }

        private static readonly int[] LogPoly3Lut8Table = {
            309628536, -534507419, 1073724054, 0,
            215207992, -502390266, 1069897914, 160852,
            158892020, -461029083, 1059680319, 1010114,
            120758300, -418592578, 1043877151, 2979626,
            93932535, -378620013, 1023979692, 6288435,
            74487828, -342313729, 1001351633, 10996073,
            60012334, -309817259, 977010327, 17079637,
            48377690, -279159893, 950059138, 24984183,
        };

        // Precision: 15.35 bits
        [MethodImpl(256)]
        public static int LogPoly3Lut8(int a) {
            var offset = (a >> 27) * 4;
            var y = Qmul30(a, LogPoly3Lut8Table[offset + 0]);
            y = Qmul30(a, y + LogPoly3Lut8Table[offset + 1]);
            y = Qmul30(a, y + LogPoly3Lut8Table[offset + 2]);
            y = y + LogPoly3Lut8Table[offset + 3];
            return y;
        }

        private static readonly int[] LogPoly5Lut8Table = {
            166189159, -263271008, 357682461, -536867223, 1073741814, 0,
            91797130, -221452381, 347549389, -535551692, 1073651718, 2559,
            55429773, -177286543, 325776420, -530104991, 1072960646, 38103,
            35101911, -139778071, 297915163, -519690478, 1071001695, 186416,
            23102252, -110088504, 268427087, -504993810, 1067326167, 555414,
            15701243, -87124604, 239861114, -487185708, 1061762610, 1252264,
            10960108, -69430156, 213404033, -467374507, 1054333366, 2368437,
            7703441, -55178389, 188423866, -445453304, 1044702281, 4063226,
        };

        // Precision: 26.22 bits
        [MethodImpl(256)]
        public static int LogPoly5Lut8(int a) {
            var offset = (a >> 27) * 6;
            var y = Qmul30(a, LogPoly5Lut8Table[offset + 0]);
            y = Qmul30(a, y + LogPoly5Lut8Table[offset + 1]);
            y = Qmul30(a, y + LogPoly5Lut8Table[offset + 2]);
            y = Qmul30(a, y + LogPoly5Lut8Table[offset + 3]);
            y = Qmul30(a, y + LogPoly5Lut8Table[offset + 4]);
            y = y + LogPoly5Lut8Table[offset + 5];
            return y;
        }

        // Log2()

        // Precision: 12.29 bits
        [MethodImpl(256)]
        public static int Log2Poly5(int a) {
            var y = Qmul30(a, 47840369); // 0.0445548155276207896995334754162140597637031202974591126199168774393873986289641382244343408731171726931757539068975485089
            y = Qmul30(a, y + -208941842); // -0.194592255208938416591621284205816720732140050852301947258138293025978577320103558315407526014074332839410207729682281855
            y = Qmul30(a, y + 450346773); // 0.419418116511448143225544710148490988337404380945888758986024844824480954559055561814904948371254539592688384332290775469
            y = Qmul30(a, y + -764275149); // -0.711786700405071059895411856470396704111669190613765834655920030051222814749610509844938951593547905280264475803542602324
            y = Qmul30(a, y + 1548771675); // 1.44240602357494054356195495511150837674248533596658656579701261211118275506108037663799064380674298602249584492474438398
            return y;
        }

        private static readonly int[] Log2Poly4Lut4Table = {
            -262388804, 497357316, -773551400, 1549073482, 0,
            -109627834, 364448809, -727169110, 1541348674, 512282,
            -55606812, 259947350, -650393145, 1515947800, 3705096,
            -30193295, 184276844, -565362946, 1473209058, 11812165,
        };

        // Precision: 17.47 bits
        [MethodImpl(256)]
        public static int Log2Poly4Lut4(int a) {
            var offset = (a >> 28) * 5;
            var y = Qmul30(a, Log2Poly4Lut4Table[offset + 0]);
            y = Qmul30(a, y + Log2Poly4Lut4Table[offset + 1]);
            y = Qmul30(a, y + Log2Poly4Lut4Table[offset + 2]);
            y = Qmul30(a, y + Log2Poly4Lut4Table[offset + 3]);
            y = y + Log2Poly4Lut4Table[offset + 4];
            return y;
        }

        private static readonly int[] Log2Poly5Lut4Table = {
            188232988, -362436158, 514145569, -774469188, 1549081618, 0,
            63930491, -229184904, 452495120, -759064000, 1547029186, 114449,
            27404630, -141534019, 367122541, -716855295, 1536437358, 1193011,
            12852334, -87700426, 286896922, -656644341, 1513678972, 4658365,
        };

        // Precision: 21.93 bits
        [MethodImpl(256)]
        public static int Log2Poly5Lut4(int a) {
            var offset = (a >> 28) * 6;
            var y = Qmul30(a, Log2Poly5Lut4Table[offset + 0]);
            y = Qmul30(a, y + Log2Poly5Lut4Table[offset + 1]);
            y = Qmul30(a, y + Log2Poly5Lut4Table[offset + 2]);
            y = Qmul30(a, y + Log2Poly5Lut4Table[offset + 3]);
            y = Qmul30(a, y + Log2Poly5Lut4Table[offset + 4]);
            y = y + Log2Poly5Lut4Table[offset + 5];
            return y;
        }

        private static readonly int[] Log2Poly3Lut8Table = {
            446326382, -771076074, 1549055308, 0,
            310260104, -724673704, 1543514571, 233309,
            229088935, -664989874, 1528754169, 1461470,
            174118266, -603771378, 1505939900, 4306814,
            135444733, -546112897, 1477222993, 9084839,
            107410065, -493744566, 1444569702, 15881168,
            86538496, -446871661, 1409446548, 24662718,
            69761446, -402649011, 1370556774, 36072616,
        };

        // Precision: 15.82 bits
        [MethodImpl(256)]
        public static int Log2Poly3Lut8(int a) {
            var offset = (a >> 27) * 4;
            var y = Qmul30(a, Log2Poly3Lut8Table[offset + 0]);
            y = Qmul30(a, y + Log2Poly3Lut8Table[offset + 1]);
            y = Qmul30(a, y + Log2Poly3Lut8Table[offset + 2]);
            y = y + Log2Poly3Lut8Table[offset + 3];
            return y;
        }

        private static readonly int[] Log2Poly3Lut16Table = {
            479498023, -773622327, 1549078527, 0,
            395931761, -759118188, 1548197526, 18808,
            334661898, -736470659, 1545381846, 136568,
            285596493, -709076642, 1540263722, 456574,
            245720905, -679311878, 1532841693, 1074840,
            212953734, -648695298, 1523292726, 2068966,
            185770248, -618189987, 1511870714, 3495916,
            163026328, -588395848, 1498851584, 5393582,
            143849516, -559673988, 1484504546, 7783737,
            127565758, -532227925, 1469077963, 10675243,
            113648249, -506157040, 1452793288, 14067055,
            101680803, -481491750, 1435843119, 17950929,
            91330868, -458215848, 1418390572, 22314023,
            82328154, -436276909, 1400565714, 27142441,
            74439828, -415566448, 1382437636, 32432624,
            67062062, -394757211, 1362869483, 38567491,
        };

        // Precision: 18.77 bits
        [MethodImpl(256)]
        public static int Log2Poly3Lut16(int a) {
            var offset = (a >> 26) * 4;
            var y = Qmul30(a, Log2Poly3Lut16Table[offset + 0]);
            y = Qmul30(a, y + Log2Poly3Lut16Table[offset + 1]);
            y = Qmul30(a, y + Log2Poly3Lut16Table[offset + 2]);
            y = y + Log2Poly3Lut16Table[offset + 3];
            return y;
        }

        private static readonly int[] Log2Poly4Lut16Table = {
            -349683705, 514860252, -774521507, 1549081965, 0,
            -271658431, 496776802, -772844764, 1549008620, 1259,
            -217158937, 469966332, -767835780, 1548587446, 14699,
            -175799370, 439219304, -759216789, 1547507699, 65699,
            -143866844, 407471403, -747343665, 1545528123, 189847,
            -118877791, 376365258, -732794890, 1542497870, 426993,
            -99090809, 346778829, -716182669, 1538346679, 816522,
            -83256460, 319137771, -698070351, 1533066538, 1394329,
            -70462839, 293601763, -678942086, 1526693477, 2191193,
            -60034672, 270176585, -659197359, 1519292323, 3232171,
            -51465396, 248781811, -639156567, 1510944906, 4536639,
            -44370441, 229291517, -619070546, 1501741200, 6118756,
            -38454405, 211558058, -599130091, 1491772420, 7988267,
            -33487114, 195423423, -579471329, 1481123710, 10151959,
            -29282549, 180709967, -560158338, 1469854024, 12618653,
            -25515190, 166551747, -540200057, 1457346639, 15558687,
        };

        // Precision: 25.20 bits
        [MethodImpl(256)]
        public static int Log2Poly4Lut16(int a) {
            var offset = (a >> 26) * 5;
            var y = Qmul30(a, Log2Poly4Lut16Table[offset + 0]);
            y = Qmul30(a, y + Log2Poly4Lut16Table[offset + 1]);
            y = Qmul30(a, y + Log2Poly4Lut16Table[offset + 2]);
            y = Qmul30(a, y + Log2Poly4Lut16Table[offset + 3]);
            y = y + Log2Poly4Lut16Table[offset + 4];
            return y;
        }

        // Sin()

        // Precision: 12.55 bits
        [MethodImpl(256)]
        public static int SinPoly2(int a) {
            var y = Qmul30(a, 78160664); // 0.072792791246675240806633584756838912025391316324690126147664432597740012658387971002826696503964998382073099859493224924
            y = Qmul30(a, y + -691048553); // -0.643589118041571860037955276396590354123911419602492412009771153095258421228154501762591444328997849123819708031503216569
            y = y + 1686629713; // 1.57079632679489661923132169163975144209852010327780228586210672049751840856976653075976474782503285074174660817200999164
            return y;
        }

        // Precision: 19.56 bits
        [MethodImpl(256)]
        public static int SinPoly3(int a) {
            var y = Qmul30(a, -4685819); // -0.00436400981703153243210864997931625819052350492882668525242722064533389220603470732171385204753335364507030843902034709469
            y = Qmul30(a, y + 85358772); // 0.0794965509242783578799016950654626792792298788902324903830739535612665082075477776612291621671450318813032241372211405835
            y = Qmul30(a, y + -693560840); // -0.645928867902143444679114736725897863187226477239208090992753453413451024571279601099280057944644528977979523870210785134
            y = y + 1686629713; // 1.57079632679489661923132169163975144209852010327780228586210672049751840856976653075976474782503285074174660817200999164
            return y;
        }

        // Precision: 27.13 bits
        [MethodImpl(256)]
        public static int SinPoly4(int a) {
            var y = Qmul30(a, 162679); // 0.000151506641710145430212560273580165931825591912723771559939880958777921352896251494433561036087921925941339032487946104446
            y = Qmul30(a, y + -5018587); // -0.0046739239118693360423625115440933405485555388758012378155538229669555462190128366781129325889847935291248353457031014355
            y = Qmul30(a, y + 85566362); // 0.0796898846149471415166275702814129714699583291426024010731469442497475447581642697337742897122044339073717901878121832219
            y = Qmul30(a, y + -693598342); // -0.645963794139684570135799310650651238951748485457327220679639722739088328461814215309859665984340413045934902046607019536
            y = y + 1686629713; // 1.57079632679489661923132169163975144209852010327780228586210672049751840856976653075976474782503285074174660817200999164
            return y;
        }

        // Atan()

        // Precision: 11.51 bits
        [MethodImpl(256)]
        public static int AtanPoly4(int a) {
            var y = Qmul30(a, 160726798); // 0.149688495302819745936382180128149414212975169816783327757105073455364913850052796368792673611118203908491930788482514717
            y = Qmul30(a, y + -389730008); // -0.3629643552067315751294669187222720090413427534177140297655271624082990667114095804438257977266614399793827935382192301
            y = Qmul30(a, y + -1791887); // -0.00166882600835556487643157555251563341464517039273944961175492629580374127544152356827950414733023151629754542508176777682
            y = Qmul30(a, y + 1074109956); // 1.00034284930971570368517715996651394929230510383744660686391316332569199570835055730032133459840273458272542980313905982
            return y;
        }

        private static readonly int[] AtanPoly5Lut8Table = {
            204464916, 1544566, -357994250, 1395, 1073741820, 0,
            119369854, 56362968, -372884915, 2107694, 1073588633, 4534,
            10771151, 190921163, -440520632, 19339556, 1071365339, 120610,
            -64491917, 329189978, -542756389, 57373179, 1064246365, 656900,
            -89925028, 390367074, -601765924, 85907899, 1057328034, 1329793,
            -80805750, 360696628, -563142238, 60762238, 1065515580, 263159,
            -58345538, 276259197, -435975641, -35140679, 1101731779, -5215389,
            -36116738, 179244146, -266417331, -183483381, 1166696761, -16608596,
            0, 0, 0, 0, 0, 843314857, // Atan(1.0)
        };

        // Precision: 28.06 bits
        [MethodImpl(256)]
        public static int AtanPoly5Lut8(int a) {
            var offset = (a >> 27) * 6;
            var y = Qmul30(a, AtanPoly5Lut8Table[offset + 0]);
            y = Qmul30(a, y + AtanPoly5Lut8Table[offset + 1]);
            y = Qmul30(a, y + AtanPoly5Lut8Table[offset + 2]);
            y = Qmul30(a, y + AtanPoly5Lut8Table[offset + 3]);
            y = Qmul30(a, y + AtanPoly5Lut8Table[offset + 4]);
            y = y + AtanPoly5Lut8Table[offset + 5];
            return y;
        }

        private static readonly int[] AtanPoly3Lut8Table = {
            -351150132, -463916, 1073745980, 0,
            -289359685, -24349242, 1076929105, -145366,
            -192305259, -97257464, 1095342438, -1708411,
            -91138684, -210466171, 1137733496, -7020039,
            -8856969, -332956892, 1198647251, -17139451,
            46187514, -435267135, 1262120294, -30283758,
            76277334, -502284461, 1311919661, -42630181,
            88081006, -532824470, 1338273149, -50214826,
            0, 0, 0, 843314857, // Atan(1.0)
        };

        // Precision: 17.98 bits
        [MethodImpl(256)]
        public static int AtanPoly3Lut8(int a) {
            var offset = (a >> 27) * 4;
            var y = Qmul30(a, AtanPoly3Lut8Table[offset + 0]);
            y = Qmul30(a, y + AtanPoly3Lut8Table[offset + 1]);
            y = Qmul30(a, y + AtanPoly3Lut8Table[offset + 2]);
            y = y + AtanPoly3Lut8Table[offset + 3];
            return y;
        }

    }

    /// <summary>
    /// Direct fixed point (signed 16.16) functions.
    /// </summary>
    public static class Fixed32 {

        public const int Shift = 16;
        public const int FractionMask = (1 << Shift) - 1;
        public const int IntegerMask = ~FractionMask;

        // Constants
        public const int Zero = 0;
        public const int Neg1 = -1 << Shift;
        public const int One = 1 << Shift;
        public const int Two = 2 << Shift;
        public const int Three = 3 << Shift;
        public const int Four = 4 << Shift;
        public const int Half = One >> 1;
        public const int Pi = (int)(13493037705L >> 16); //(int)(Math.PI * 65536.0) << 16;
        public const int Pi2 = (int)(26986075409L >> 16);
        public const int PiHalf = (int)(6746518852L >> 16);
        public const int PiOver4 = (int)(3373259426L >> 16);
        public const int E = (int)(11674931555L >> 16);
        public const int Sqrt2 = 92681; // sqrt(2)

        public const int MinValue = -2147483648;
        public const int MaxValue = 2147483647;

        public const int RCP_LN2 = (int)(0x171547652L >> 16); // 1.0 / log(2.0) ~= 1.4426950408889634
        public const int RCP_LOG2_E = (int)(2977044471L >> 16); // 1.0 / log2(e) ~= 0.6931471805599453
        public const int RCP_TWO_PI = 683565276; // 1.0 / (4.0 * 0.5 * pi);  -- the 4.0 factor converts directly to s2.30

        public const float M = 1.0f / 65536.0f;

        /// <summary>
        /// Converts an integer to a fixed-point value.
        /// </summary>
        [MethodImpl(256)]
        public static int FromInt(int v) {
            return (int)v << Shift;
        }

        /// <summary>
        /// Converts a double to a fixed-point value.
        /// </summary>
        [MethodImpl(256)]
        public static int FromDouble(double v) {
            return (int)(v * 65536.0);
        }

        /// <summary>
        /// Converts a float to a fixed-point value.
        /// </summary>
        [MethodImpl(256)]
        public static int FromFloat(float v) {
            return (int)(v * 65536.0f);
        }

        /// <summary>
        /// Converts a fixed-point value into an integer by rounding it up to nearest integer.
        /// </summary>
        [MethodImpl(256)]
        public static int CeilToInt(int v) {
            return (int)((v + (One - 1)) >> Shift);
        }

        /// <summary>
        /// Converts a fixed-point value into an integer by rounding it down to nearest integer.
        /// </summary>
        [MethodImpl(256)]
        public static int FloorToInt(int v) {
            return (int)(v >> Shift);
        }

        /// <summary>
        /// Converts a fixed-point value into an integer by rounding it to nearest integer.
        /// </summary>
        [MethodImpl(256)]
        public static int RoundToInt(int v) {
            return (int)((v + Half) >> Shift);
        }

        /// <summary>
        /// Bit conversion of raw value to int
        /// </summary>
        /// <param name="rawValue"></param>
        /// <returns></returns>
        [MethodImpl(256)]
        public static int AsInt(int rawValue) {
            var x = (float)sfloat.FromRaw(rawValue);
            unsafe {
                return *(int*)&x;
            }
        }

        /// <summary>
        /// Bit conversion of raw value to int
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(256)]
        public static int AsFloat(int value) {
            var rawValue = (int)sfloat.FromRaw(FromInt(value));
            unsafe {
                return FromFloat(*(float*)&rawValue);
            }
        }

        /// <summary>
        /// Converts a fixed-point value into a double.
        /// </summary>
        [MethodImpl(256)]
        public static double ToDouble(int v) {
            return (double)v * (1.0 / 65536.0);
        }

        /// <summary>
        /// Converts a FP value into a float.
        /// </summary>
        [MethodImpl(256)]
        public static float ToFloat(int v) {
            return (float)v * M;
        }

        /// <summary>
        /// Converts the value to a human readable string.
        /// </summary>
        public static string ToString(int v) {
            return ToDouble(v).ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Returns the absolute (positive) value of x.
        /// </summary>
        [MethodImpl(256)]
        public static int Abs(int x) {
            // \note fails with MinValue
            var mask = x >> 31;
            return (x + mask) ^ mask;
        }

        /// <summary>
        /// Negative absolute value (returns -abs(x)).
        /// </summary>
        [MethodImpl(256)]
        public static int Nabs(int x) {
            return -Abs(x);
        }

        /// <summary>
        /// Round up to nearest integer.
        /// </summary>
        [MethodImpl(256)]
        public static int Ceil(int x) {
            return (x + FractionMask) & IntegerMask;
        }

        /// <summary>
        /// Round down to nearest integer.
        /// </summary>
        [MethodImpl(256)]
        public static int Floor(int x) {
            return x & IntegerMask;
        }

        /// <summary>
        /// Round to nearest integer.
        /// </summary>
        [MethodImpl(256)]
        public static int Round(int x) {
            return (x + Half) & IntegerMask;
        }

        /// <summary>
        /// Returns the fractional part of x. Equal to 'x - floor(x)'.
        /// </summary>
        [MethodImpl(256)]
        public static int Fract(int x) {
            return x & FractionMask;
        }

        /// <summary>
        /// Returns the minimum of the two values.
        /// </summary>
        [MethodImpl(256)]
        public static int Min(int a, int b) {
            return a < b ? a : b;
        }

        /// <summary>
        /// Returns the maximum of the two values.
        /// </summary>
        [MethodImpl(256)]
        public static int Max(int a, int b) {
            return a > b ? a : b;
        }

        /// <summary>
        /// Returns the value clamped between min and max.
        /// </summary>
        [MethodImpl(256)]
        public static int Clamp(int a, int min, int max) {
            return a > max ? max : a < min ? min : a;
        }

        /// <summary>
        /// Returns the sign of the value (-1 if negative, 0 if zero, 1 if positive).
        /// </summary>
        [MethodImpl(256)]
        public static int Sign(int x) {
            // https://stackoverflow.com/questions/14579920/fast-sign-of-integer-in-c/14612418#14612418
            #if JAVA
            return ((x >> 31) | (-x >>> 31));
            #else
            return (x >> 31) | (int)((uint)-x >> 31);
            #endif
        }

        /// <summary>
        /// Adds the two FP numbers together.
        /// </summary>
        [MethodImpl(256)]
        public static int Add(int a, int b) {
            return a + b;
        }

        /// <summary>
        /// Subtracts the two FP numbers from each other.
        /// </summary>
        [MethodImpl(256)]
        public static int Sub(int a, int b) {
            return a - b;
        }

        /// <summary>
        /// Multiplies two FP values together.
        /// </summary>
        [MethodImpl(256)]
        public static int Mul(int a, int b) {
            return (int)(((long)a * (long)b) >> Shift);
        }

        /// <summary>
        /// Linearly interpolate from a to b by t.
        /// </summary>
        [MethodImpl(256)]
        public static int Lerp(int a, int b, int t) {
            var ta = (long)a * (One - (long)t);
            var tb = (long)b * (long)t;
            return (int)((ta + tb) >> Shift);
        }

        #if JAVA
        private static int Nlz(int x)
        {
            return Integer.numberOfLeadingZeros(x);
        }
        #else
        [MethodImpl(256)]
        private static int Nlz(uint x) {
            //return System.Numerics.BitOperations.LeadingZeroCount(x); \note Disabled as this is slower in benchmarks
            var n = 0;
            if (x <= 0x0000FFFF) {
                n = n + 16;
                x = x << 16;
            }

            if (x <= 0x00FFFFFF) {
                n = n + 8;
                x = x << 8;
            }

            if (x <= 0x0FFFFFFF) {
                n = n + 4;
                x = x << 4;
            }

            if (x <= 0x3FFFFFFF) {
                n = n + 2;
                x = x << 2;
            }

            if (x <= 0x7FFFFFFF) {
                n = n + 1;
            }

            if (x == 0) {
                return 32;
            }

            return n;
        }
        #endif

        /// <summary>
        /// Divides two FP values.
        /// </summary>
        public static int DivPrecise(int a, int b) {
            if (b == MinValue || b == 0) {
                return 0;
            }

            var res = (int)(((long)a << Shift) / (long)b);
            return res;
        }

        /// <summary>
        /// Calculates division approximation.
        /// </summary>
        public static int Div(int a, int b) {
            if (b == MinValue || b == 0) {
                FixedUtil.InvalidArgument("Fixed32.Div", "b", b);
                return 0;
            }

            return (int)(((long)a << 16) / b);
        }

        /// <summary>
        /// Calculates division approximation.
        /// </summary>
        public static int DivFast(int a, int b) {
            if (b == MinValue || b == 0) {
                FixedUtil.InvalidArgument("Fixed32.DivFast", "b", b);
                return 0;
            }

            // Handle negative values.
            var sign = b < 0 ? -1 : 1;
            b *= sign;

            // Normalize input into [1.0, 2.0( range (convert to s2.30).
            var offset = 29 - Nlz((uint)b);
            var n = FixedUtil.ShiftRight(b, offset - 28);
            const int ONE = 1 << 30;
            Debug.Assert(n >= ONE);

            // Polynomial approximation.
            var res = FixedUtil.RcpPoly6(n - ONE);

            // Multiply by reciprocal, apply exponent, convert back to s16.16.
            var y = FixedUtil.Qmul30(res, a);
            return FixedUtil.ShiftRight(sign * y, offset - 14);
        }

        /// <summary>
        /// Calculates division approximation.
        /// </summary>
        public static int DivFastest(int a, int b) {
            if (b == MinValue || b == 0) {
                FixedUtil.InvalidArgument("Fixed32.DivFastest", "b", b);
                return 0;
            }

            // Handle negative values.
            var sign = b < 0 ? -1 : 1;
            b *= sign;

            // Normalize input into [1.0, 2.0( range (convert to s2.30).
            var offset = 29 - Nlz((uint)b);
            var n = FixedUtil.ShiftRight(b, offset - 28);
            const int ONE = 1 << 30;
            Debug.Assert(n >= ONE);

            // Polynomial approximation.
            var res = FixedUtil.RcpPoly4(n - ONE);

            // Multiply by reciprocal, apply exponent, convert back to s16.16.
            var y = FixedUtil.Qmul30(res, a);
            return FixedUtil.ShiftRight(sign * y, offset - 14);
        }

        /// <summary>
        /// Divides two FP values and returns the modulus.
        /// </summary>
        public static int Mod(int a, int b) {
            if (b == 0) {
                FixedUtil.InvalidArgument("Fixed32.Mod", "b", b);
                return 0;
            }

            return a % b;
        }

        /// <summary>
        /// Calculates the square root of the given number.
        /// </summary>
        public static int SqrtPrecise(int a) {
            // Adapted from https://github.com/chmike/fpsqrt
            if (a <= 0) {
                if (a < 0) {
                    FixedUtil.InvalidArgument("Fixed32.SqrtPrecise", "a", a);
                }

                return 0;
            }

            #if JAVA
            int r = a;
            int b = 0x40000000;
            int q = 0;
            while (b > 0x40)
            {
                int t = q + b;
                if (Integer.compareUnsigned(r, t) >= 0)
                {
                    r -= t;
                    q = t + b;
                }
                r <<= 1;
                b >>= 1;
            }
            q >>>= 8;
            return q;
            #else
            var r = (uint)a;
            uint b = 0x40000000;
            uint q = 0;
            while (b > 0x40) {
                var t = q + b;
                if (r >= t) {
                    r -= t;
                    q = t + b;
                }

                r <<= 1;
                b >>= 1;
            }

            q >>= 8;
            return (int)q;
            #endif
        }

        public static int Sqrt(int x) {
            // Return 0 for all non-positive values.
            if (x <= 0) {
                if (x < 0) {
                    FixedUtil.InvalidArgument("Fixed32.Sqrt", "x", x);
                }

                return 0;
            }

            // Constants (s2.30).
            const int ONE = 1 << 30;
            const int SQRT2 = 1518500249; // sqrt(2.0)

            // Normalize input into [1.0, 2.0( range (as s2.30).
            var offset = 15 - Nlz((uint)x);
            var n = FixedUtil.ShiftRight(x, offset - 14);
            Debug.Assert(n >= ONE);
            var y = FixedUtil.SqrtPoly3Lut8(n - ONE);

            // Divide offset by 2 (to get sqrt), compute adjust value for odd exponents.
            var adjust = (offset & 1) != 0 ? SQRT2 : ONE;
            offset = offset >> 1;

            // Apply exponent, convert back to s16.16.
            var yr = FixedUtil.Qmul30(adjust, y);
            return FixedUtil.ShiftRight(yr, 14 - offset);
        }

        public static int SqrtFast(int x) {
            // Return 0 for all non-positive values.
            if (x <= 0) {
                if (x < 0) {
                    FixedUtil.InvalidArgument("Fixed32.SqrtFast", "x", x);
                }

                return 0;
            }

            // Constants (s2.30).
            const int ONE = 1 << 30;
            const int SQRT2 = 1518500249; // sqrt(2.0)

            // Normalize input into [1.0, 2.0( range (as s2.30).
            var offset = 15 - Nlz((uint)x);
            var n = FixedUtil.ShiftRight(x, offset - 14);
            Debug.Assert(n >= ONE);
            var y = FixedUtil.SqrtPoly4(n - ONE);

            // Divide offset by 2 (to get sqrt), compute adjust value for odd exponents.
            var adjust = (offset & 1) != 0 ? SQRT2 : ONE;
            offset = offset >> 1;

            // Apply exponent, convert back to s16.16.
            var yr = FixedUtil.Qmul30(adjust, y);
            return FixedUtil.ShiftRight(yr, 14 - offset);
        }

        public static int SqrtFastest(int x) {
            // Return 0 for all non-positive values.
            if (x <= 0) {
                if (x < 0) {
                    FixedUtil.InvalidArgument("Fixed32.SqrtFastest", "x", x);
                }

                return 0;
            }

            // Constants (s2.30).
            const int ONE = 1 << 30;
            const int SQRT2 = 1518500249; // sqrt(2.0)

            // Normalize input into [1.0, 2.0( range (as s2.30).
            var offset = 15 - Nlz((uint)x);
            var n = FixedUtil.ShiftRight(x, offset - 14);
            Debug.Assert(n >= ONE);
            var y = FixedUtil.SqrtPoly3(n - ONE);

            // Divide offset by 2 (to get sqrt), compute adjust value for odd exponents.
            var adjust = (offset & 1) != 0 ? SQRT2 : ONE;
            offset = offset >> 1;

            // Apply exponent, convert back to s16.16.
            var yr = FixedUtil.Qmul30(adjust, y);
            return FixedUtil.ShiftRight(yr, 14 - offset);
        }

        /// <summary>
        /// Calculates the reciprocal square root.
        /// </summary>
        public static int RSqrt(int x) {
            // Return 0 for invalid values
            if (x <= 0) {
                FixedUtil.InvalidArgument("Fixed32.RSqrt", "x", x);
                return 0;
            }

            // Constants (s2.30).
            const int ONE = 1 << 30;
            const int HALF_SQRT2 = 759250125; // 0.5 * sqrt(2.0)

            // Normalize input into [1.0, 2.0( range (as s2.30).
            var offset = 1 - Nlz((uint)x);
            var n = FixedUtil.ShiftRight(x, offset);
            Debug.Assert(n >= ONE);
            var y = FixedUtil.RSqrtPoly3Lut16(n - ONE);

            // Divide offset by 2 (to get sqrt), compute adjust value for odd exponents.
            var adjust = (offset & 1) != 0 ? HALF_SQRT2 : ONE;
            offset = offset >> 1;

            // Apply exponent, convert back to s16.16.
            var yr = FixedUtil.Qmul30(adjust, y);
            return FixedUtil.ShiftRight(yr, offset + 21);
        }

        /// <summary>
        /// Calculates the reciprocal square root.
        /// </summary>
        public static int RSqrtFast(int x) {
            // Return 0 for invalid values
            if (x <= 0) {
                FixedUtil.InvalidArgument("Fixed32.RSqrtFast", "x", x);
                return 0;
            }

            // Constants (s2.30).
            const int ONE = 1 << 30;
            const int HALF_SQRT2 = 759250125; // 0.5 * sqrt(2.0)

            // Normalize input into [1.0, 2.0( range (as s2.30).
            var offset = 1 - Nlz((uint)x);
            var n = FixedUtil.ShiftRight(x, offset);
            Debug.Assert(n >= ONE);
            var y = FixedUtil.RSqrtPoly5(n - ONE);

            // Divide offset by 2 (to get sqrt), compute adjust value for odd exponents.
            var adjust = (offset & 1) != 0 ? HALF_SQRT2 : ONE;
            offset = offset >> 1;

            // Apply exponent, convert back to s16.16.
            var yr = FixedUtil.Qmul30(adjust, y);
            return FixedUtil.ShiftRight(yr, offset + 21);
        }

        /// <summary>
        /// Calculates the reciprocal square root.
        /// </summary>
        public static int RSqrtFastest(int x) {
            // Return 0 for invalid values
            if (x <= 0) {
                FixedUtil.InvalidArgument("Fixed32.RSqrtFastest", "x", x);
                return 0;
            }

            // Constants (s2.30).
            const int ONE = 1 << 30;
            const int HALF_SQRT2 = 759250125; // 0.5 * sqrt(2.0)

            // Normalize input into [1.0, 2.0( range (as s2.30).
            var offset = 1 - Nlz((uint)x);
            var n = FixedUtil.ShiftRight(x, offset);
            Debug.Assert(n >= ONE);
            var y = FixedUtil.RSqrtPoly3(n - ONE);

            // Divide offset by 2 (to get sqrt), compute adjust value for odd exponents.
            var adjust = (offset & 1) != 0 ? HALF_SQRT2 : ONE;
            offset = offset >> 1;

            // Apply exponent, convert back to s16.16.
            var yr = FixedUtil.Qmul30(adjust, y);
            return FixedUtil.ShiftRight(yr, offset + 21);
        }

        /// <summary>
        /// Calculates reciprocal approximation.
        /// </summary>
        public static int Rcp(int x) {
            if (x == MinValue || x == 0) {
                FixedUtil.InvalidArgument("Fixed32.Rcp", "x", x);
                return 0;
            }

            // Handle negative values.
            var sign = x < 0 ? -1 : 1;
            x *= sign;

            // Normalize input into [1.0, 2.0( range (convert to s2.30).
            var offset = 29 - Nlz((uint)x);
            var n = FixedUtil.ShiftRight(x, offset - 28);
            const int ONE = 1 << 30;
            Debug.Assert(n >= ONE);

            // Polynomial approximation.
            var res = FixedUtil.RcpPoly4Lut8(n - ONE);

            // Apply exponent, convert back to s16.16.
            return FixedUtil.ShiftRight(sign * res, offset);
        }

        /// <summary>
        /// Calculates reciprocal approximation.
        /// </summary>
        public static int RcpFast(int x) {
            if (x == MinValue || x == 0) {
                FixedUtil.InvalidArgument("Fixed32.RcpFast", "x", x);
                return 0;
            }

            // Handle negative values.
            var sign = x < 0 ? -1 : 1;
            x *= sign;

            // Normalize input into [1.0, 2.0( range (convert to s2.30).
            var offset = 29 - Nlz((uint)x);
            var n = FixedUtil.ShiftRight(x, offset - 28);
            const int ONE = 1 << 30;
            Debug.Assert(n >= ONE);

            // Polynomial approximation.
            var res = FixedUtil.RcpPoly6(n - ONE);
            //int res = Util.RcpPoly3Lut8(n - ONE);

            // Apply exponent, convert back to s16.16.
            return FixedUtil.ShiftRight(sign * res, offset);
        }

        /// <summary>
        /// Calculates reciprocal approximation.
        /// </summary>
        public static int RcpFastest(int x) {
            if (x == MinValue || x == 0) {
                FixedUtil.InvalidArgument("Fixed32.RcpFastest", "x", x);
                return 0;
            }

            // Handle negative values.
            var sign = x < 0 ? -1 : 1;
            x *= sign;

            // Normalize input into [1.0, 2.0( range (convert to s2.30).
            var offset = 29 - Nlz((uint)x);
            var n = FixedUtil.ShiftRight(x, offset - 28);
            const int ONE = 1 << 30;
            Debug.Assert(n >= ONE);

            // Polynomial approximation.
            var res = FixedUtil.RcpPoly4(n - ONE);
            //int res = Util.RcpPoly3Lut4(n - ONE);

            // Apply exponent, convert back to s16.16.
            return FixedUtil.ShiftRight(sign * res, offset);
        }

        /// <summary>
        /// Calculates the base 2 exponent.
        /// </summary>
        public static int Exp2(int x) {
            // Handle values that would under or overflow.
            if (x >= 15 * One) {
                return MaxValue;
            }

            if (x <= -16 * One) {
                return 0;
            }

            // Compute exp2 for fractional part.
            var k = (x & FractionMask) << 14;
            var y = FixedUtil.Exp2Poly5(k);

            // Combine integer and fractional result, and convert back to s16.16.
            var intPart = x >> Shift;
            return FixedUtil.ShiftRight(y, 14 - intPart);
        }

        /// <summary>
        /// Calculates the base 2 exponent.
        /// </summary>
        public static int Exp2Fast(int x) {
            // Handle values that would under or overflow.
            if (x >= 15 * One) {
                return MaxValue;
            }

            if (x <= -16 * One) {
                return 0;
            }

            // Compute exp2 for fractional part.
            var k = (x & FractionMask) << 14;
            var y = FixedUtil.Exp2Poly4(k);

            // Combine integer and fractional result, and convert back to s16.16.
            var intPart = x >> Shift;
            return FixedUtil.ShiftRight(y, 14 - intPart);
        }

        /// <summary>
        /// Calculates the base 2 exponent.
        /// </summary>
        public static int Exp2Fastest(int x) {
            // Handle values that would under or overflow.
            if (x >= 15 * One) {
                return MaxValue;
            }

            if (x <= -16 * One) {
                return 0;
            }

            // Compute exp2 for fractional part.
            var k = (x & FractionMask) << 14;
            var y = FixedUtil.Exp2Poly3(k);

            // Combine integer and fractional result, and convert back to s16.16.
            var intPart = x >> Shift;
            return FixedUtil.ShiftRight(y, 14 - intPart);
        }

        public static int Exp(int x) {
            // e^x == 2^(x / ln(2))
            return Exp2(Mul(x, RCP_LN2));
        }

        public static int ExpFast(int x) {
            // e^x == 2^(x / ln(2))
            return Exp2Fast(Mul(x, RCP_LN2));
        }

        public static int ExpFastest(int x) {
            // e^x == 2^(x / ln(2))
            return Exp2Fastest(Mul(x, RCP_LN2));
        }

        public static int Log(int x) {
            // Return 0 for invalid values
            if (x <= 0) {
                FixedUtil.InvalidArgument("Fixed32.Log", "x", x);
                return 0;
            }

            // Normalize value to range [1.0, 2.0( as s2.30 and extract exponent.
            var offset = 15 - Nlz((uint)x);
            var n = FixedUtil.ShiftRight(x, offset - 14);

            // Polynomial approximation.
            const int ONE = 1 << 30;
            Debug.Assert(n >= ONE);
            var y = FixedUtil.LogPoly5Lut8(n - ONE);

            // Combine integer and fractional parts (into s16.16).
            return offset * RCP_LOG2_E + (y >> 14);
        }

        public static int LogFast(int x) {
            // Return 0 for invalid values
            if (x <= 0) {
                FixedUtil.InvalidArgument("Fixed32.LogFast", "x", x);
                return 0;
            }

            // Normalize value to range [1.0, 2.0( as s2.30 and extract exponent.
            var offset = 15 - Nlz((uint)x);
            var n = FixedUtil.ShiftRight(x, offset - 14);

            // Polynomial approximation.
            const int ONE = 1 << 30;
            Debug.Assert(n >= ONE);
            var y = FixedUtil.LogPoly3Lut8(n - ONE);

            // Combine integer and fractional parts (into s16.16).
            return offset * RCP_LOG2_E + (y >> 14);
        }

        public static int LogFastest(int x) {
            // Return 0 for invalid values
            if (x <= 0) {
                FixedUtil.InvalidArgument("Fixed32.LogFastest", "x", x);
                return 0;
            }

            // Normalize value to range [1.0, 2.0( as s2.30 and extract exponent.
            var offset = 15 - Nlz((uint)x);
            var n = FixedUtil.ShiftRight(x, offset - 14);

            // Polynomial approximation.
            const int ONE = 1 << 30;
            Debug.Assert(n >= ONE);
            var y = FixedUtil.LogPoly5(n - ONE);

            // Combine integer and fractional parts (into s16.16).
            return offset * RCP_LOG2_E + (y >> 14);
        }

        public static int Log2(int x) {
            // Return 0 for invalid values
            if (x <= 0) {
                FixedUtil.InvalidArgument("Fixed32.Log2", "x", x);
                return 0;
            }

            // Normalize value to range [1.0, 2.0( as s2.30 and extract exponent.
            var offset = 15 - Nlz((uint)x);
            var n = FixedUtil.ShiftRight(x, offset - 14);

            // Polynomial approximation of mantissa.
            const int ONE = 1 << 30;
            Debug.Assert(n >= ONE);
            var y = FixedUtil.Log2Poly4Lut16(n - ONE);

            // Combine integer and fractional parts (into s16.16).
            return (offset << Shift) + (y >> 14);
        }

        public static int Log2Fast(int x) {
            // Return 0 for invalid values
            if (x <= 0) {
                FixedUtil.InvalidArgument("Fixed32.Log2Fast", "x", x);
                return 0;
            }

            // Normalize value to range [1.0, 2.0( as s2.30 and extract exponent.
            var offset = 15 - Nlz((uint)x);
            var n = FixedUtil.ShiftRight(x, offset - 14);

            // Polynomial approximation of mantissa.
            const int ONE = 1 << 30;
            Debug.Assert(n >= ONE);
            var y = FixedUtil.Log2Poly3Lut16(n - ONE);

            // Combine integer and fractional parts (into s16.16).
            return (offset << Shift) + (y >> 14);
        }

        public static int Log2Fastest(int x) {
            // Return 0 for invalid values
            if (x <= 0) {
                FixedUtil.InvalidArgument("Fixed32.Log2Fastest", "x", x);
                return 0;
            }

            // Normalize value to range [1.0, 2.0( as s2.30 and extract exponent.
            var offset = 15 - Nlz((uint)x);
            var n = FixedUtil.ShiftRight(x, offset - 14);

            // Polynomial approximation of mantissa.
            const int ONE = 1 << 30;
            Debug.Assert(n >= ONE);
            var y = FixedUtil.Log2Poly5(n - ONE);

            // Combine integer and fractional parts (into s16.16).
            return (offset << Shift) + (y >> 14);
        }

        /// <summary>
        /// Calculates x to the power of the exponent.
        /// </summary>
        public static int Pow(int x, int exponent) {
            // n^0 == 1
            if (exponent == 0) {
                return One;
            }

            // Return 0 for invalid values
            if (x <= 0) {
                if (x < 0) {
                    FixedUtil.InvalidArgument("Fixed32.Pow", "x", x);
                }

                return 0;
            }

            return Exp(Mul(exponent, Log(x)));
        }

        /// <summary>
        /// Calculates x to the power of the exponent.
        /// </summary>
        public static int PowFast(int x, int exponent) {
            // n^0 == 1
            if (exponent == 0) {
                return One;
            }

            // Return 0 for invalid values
            if (x <= 0) {
                if (x < 0) {
                    FixedUtil.InvalidArgument("Fixed32.PowFast", "x", x);
                }

                return 0;
            }

            return ExpFast(Mul(exponent, LogFast(x)));
        }

        /// <summary>
        /// Calculates x to the power of the exponent.
        /// </summary>
        public static int PowFastest(int x, int exponent) {
            // n^0 == 1
            if (exponent == 0) {
                return One;
            }

            // Return 0 for invalid values
            if (x <= 0) {
                if (x < 0) {
                    FixedUtil.InvalidArgument("Fixed32.PowFastest", "x", x);
                }

                return 0;
            }

            return ExpFastest(Mul(exponent, LogFastest(x)));
        }

        [MethodImpl(256)]
        private static int UnitSin(int z) {
            // See: http://www.coranac.com/2009/07/sines/

            // Handle quadrants 1 and 2 by mirroring the [1, 3] range to [-1, 1] (by calculating 2 - z).
            // The if condition uses the fact that for the quadrants of interest are 0b01 and 0b10 (top two bits are different).
            if ((z ^ (z << 1)) < 0) {
                z = (1 << 31) - z;
            }

            // Now z is in range [-1, 1].
            const int ONE = 1 << 30;
            Debug.Assert(z >= -ONE && z <= ONE);

            // Polynomial approximation.
            var zz = FixedUtil.Qmul30(z, z);
            var res = FixedUtil.Qmul30(FixedUtil.SinPoly4(zz), z);

            // Return as s2.30.
            return res;
        }

        [MethodImpl(256)]
        private static int UnitSinFast(int z) {
            // See: http://www.coranac.com/2009/07/sines/

            // Handle quadrants 1 and 2 by mirroring the [1, 3] range to [-1, 1] (by calculating 2 - z).
            // The if condition uses the fact that for the quadrants of interest are 0b01 and 0b10 (top two bits are different).
            if ((z ^ (z << 1)) < 0) {
                z = (1 << 31) - z;
            }

            // Now z is in range [-1, 1].
            const int ONE = 1 << 30;
            Debug.Assert(z >= -ONE && z <= ONE);

            // Polynomial approximation.
            var zz = FixedUtil.Qmul30(z, z);
            var res = FixedUtil.Qmul30(FixedUtil.SinPoly3(zz), z);

            // Return as s2.30.
            return res;
        }

        [MethodImpl(256)]
        private static int UnitSinFastest(int z) {
            // See: http://www.coranac.com/2009/07/sines/

            // Handle quadrants 1 and 2 by mirroring the [1, 3] range to [-1, 1] (by calculating 2 - z).
            // The if condition uses the fact that for the quadrants of interest are 0b01 and 0b10 (top two bits are different).
            if ((z ^ (z << 1)) < 0) {
                z = (1 << 31) - z;
            }

            // Now z is in range [-1, 1].
            const int ONE = 1 << 30;
            Debug.Assert(z >= -ONE && z <= ONE);

            // Polynomial approximation.
            var zz = FixedUtil.Qmul30(z, z);
            var res = FixedUtil.Qmul30(FixedUtil.SinPoly2(zz), z);

            // Return as s2.30.
            return res;
        }

        public static int Sin(int x) {
            // Map [0, 2pi] to [0, 4] (as s2.30).
            // This also wraps the values into one period.
            var z = Mul(RCP_TWO_PI, x);

            // Compute sin from s2.30 and convert back to s16.16.
            return UnitSin(z) >> 14;
        }

        public static int SinFast(int x) {
            // Map [0, 2pi] to [0, 4] (as s2.30).
            // This also wraps the values into one period.
            var z = Mul(RCP_TWO_PI, x);

            // Compute sin from s2.30 and convert back to s16.16.
            return UnitSinFast(z) >> 14;
        }

        public static int SinFastest(int x) {
            // Map [0, 2pi] to [0, 4] (as s2.30).
            // This also wraps the values into one period.
            var z = Mul(RCP_TWO_PI, x);

            // Compute sin from s2.30 and convert back to s16.16.
            return UnitSinFastest(z) >> 14;
        }

        [MethodImpl(256)]
        public static int Cos(int x) {
            return Sin(x + PiHalf);
        }

        [MethodImpl(256)]
        public static int CosFast(int x) {
            return SinFast(x + PiHalf);
        }

        [MethodImpl(256)]
        public static int CosFastest(int x) {
            return SinFastest(x + PiHalf);
        }

        public static int Tan(int x) {
            var z = Mul(RCP_TWO_PI, x);
            var sinX = UnitSin(z);
            var cosX = UnitSin(z + (1 << 30));
            return Div(sinX, cosX);
        }

        public static int TanFast(int x) {
            var z = Mul(RCP_TWO_PI, x);
            var sinX = UnitSinFast(z);
            var cosX = UnitSinFast(z + (1 << 30));
            return DivFast(sinX, cosX);
        }

        public static int TanFastest(int x) {
            var z = Mul(RCP_TWO_PI, x);
            var sinX = UnitSinFastest(z);
            var cosX = UnitSinFastest(z + (1 << 30));
            return DivFastest(sinX, cosX);
        }

        private static int Atan2Div(int y, int x) {
            Debug.Assert(y >= 0 && x > 0 && x >= y);

            // Normalize input into [1.0, 2.0( range (convert to s2.30).
            const int ONE = 1 << 30;
            const int HALF = 1 << 29;
            var offset = 1 - Nlz((uint)x);
            var n = FixedUtil.ShiftRight(x, offset);
            Debug.Assert(n >= ONE);

            // Polynomial approximation of reciprocal.
            var oox = FixedUtil.RcpPoly4Lut8(n - ONE);
            Debug.Assert(oox >= HALF && oox <= ONE);

            // Apply exponent and multiply.
            var yr = FixedUtil.ShiftRight(y, offset);
            return FixedUtil.Qmul30(yr, oox);
        }

        public static int Atan2(int y, int x) {
            // See: https://www.dsprelated.com/showarticle/1052.php

            if (x == 0) {
                if (y > 0) {
                    return PiHalf;
                }

                if (y < 0) {
                    return -PiHalf;
                }

                FixedUtil.InvalidArgument("Fixed32.Atan2", "y, x", y, x);
                return 0;
            }

            var nx = Abs(x);
            var ny = Abs(y);
            var negMask = (x ^ y) >> 31;

            if (nx >= ny) {
                var k = Atan2Div(ny, nx);
                var z = FixedUtil.AtanPoly5Lut8(k);
                var angle = (negMask ^ (z >> 14)) - negMask;
                if (x > 0) {
                    return angle;
                }

                if (y >= 0) {
                    return angle + Pi;
                }

                return angle - Pi;
            } else {
                var k = Atan2Div(nx, ny);
                var z = FixedUtil.AtanPoly5Lut8(k);
                var angle = negMask ^ (z >> 14);
                return (y > 0 ? PiHalf : -PiHalf) - angle;
            }
        }

        private static int Atan2DivFast(int y, int x) {
            Debug.Assert(y >= 0 && x > 0 && x >= y);

            // Normalize input into [1.0, 2.0( range (convert to s2.30).
            const int ONE = 1 << 30;
            const int HALF = 1 << 29;
            var offset = 1 - Nlz((uint)x);
            var n = FixedUtil.ShiftRight(x, offset);

            // Polynomial approximation.
            var oox = FixedUtil.RcpPoly6(n - ONE);
            Debug.Assert(oox >= HALF && oox <= ONE);

            // Apply exponent and multiply.
            var yr = FixedUtil.ShiftRight(y, offset);
            return FixedUtil.Qmul30(yr, oox);
        }

        public static int Atan2Fast(int y, int x) {
            // See: https://www.dsprelated.com/showarticle/1052.php

            if (x == 0) {
                if (y > 0) {
                    return PiHalf;
                }

                if (y < 0) {
                    return -PiHalf;
                }

                FixedUtil.InvalidArgument("Fixed32.Atan2Fast", "y, x", y, x);
                return 0;
            }

            var nx = Abs(x);
            var ny = Abs(y);
            var negMask = (x ^ y) >> 31;

            if (nx >= ny) {
                var k = Atan2DivFast(ny, nx);
                var z = FixedUtil.AtanPoly3Lut8(k);
                var angle = negMask ^ (z >> 14);
                if (x > 0) {
                    return angle;
                }

                if (y >= 0) {
                    return angle + Pi;
                }

                return angle - Pi;
            } else {
                var k = Atan2DivFast(nx, ny);
                var z = FixedUtil.AtanPoly3Lut8(k);
                var angle = negMask ^ (z >> 14);
                return (y > 0 ? PiHalf : -PiHalf) - angle;
            }
        }

        private static int Atan2DivFastest(int y, int x) {
            Debug.Assert(y >= 0 && x > 0 && x >= y);

            // Normalize input into [1.0, 2.0( range (convert to s2.30).
            const int ONE = 1 << 30;
            const int HALF = 1 << 29;
            var offset = 1 - Nlz((uint)x);
            var n = FixedUtil.ShiftRight(x, offset);

            // Polynomial approximation.
            var oox = FixedUtil.RcpPoly4(n - ONE);
            Debug.Assert(oox >= HALF && oox <= ONE);

            // Apply exponent and multiply.
            var yr = FixedUtil.ShiftRight(y, offset);
            return FixedUtil.Qmul30(yr, oox);
        }

        public static int Atan2Fastest(int y, int x) {
            // See: https://www.dsprelated.com/showarticle/1052.php

            if (x == 0) {
                if (y > 0) {
                    return PiHalf;
                }

                if (y < 0) {
                    return -PiHalf;
                }

                FixedUtil.InvalidArgument("Fixed32.Atan2Fastest", "y, x", y, x);
                return 0;
            }

            var nx = Abs(x);
            var ny = Abs(y);
            var negMask = (x ^ y) >> 31;

            if (nx >= ny) {
                var k = Atan2DivFastest(ny, nx);
                var z = FixedUtil.AtanPoly4(k);
                var angle = negMask ^ (z >> 14);
                if (x > 0) {
                    return angle;
                }

                if (y >= 0) {
                    return angle + Pi;
                }

                return angle - Pi;
            } else {
                var k = Atan2DivFastest(nx, ny);
                var z = FixedUtil.AtanPoly4(k);
                var angle = negMask ^ (z >> 14);
                return (y > 0 ? PiHalf : -PiHalf) - angle;
            }
        }

        public static int Asin(int x) {
            // Return 0 for invalid values
            if (x < -One || x > One) {
                FixedUtil.InvalidArgument("Fixed32.Asin", "x", x);
                return 0;
            }

            // Compute Atan2(x, Sqrt((1+x) * (1-x))), using s32.32.
            var xx = (long)(One + x) * (long)(One - x);
            var y = Fixed64.Sqrt(xx);
            return (int)(Fixed64.Atan2((long)x << 16, y) >> 16);
        }

        public static int AsinFast(int x) {
            // Return 0 for invalid values
            if (x < -One || x > One) {
                FixedUtil.InvalidArgument("Fixed32.AsinFast", "x", x);
                return 0;
            }

            // Compute Atan2(x, Sqrt((1+x) * (1-x))), using s32.32.
            var xx = (long)(One + x) * (long)(One - x);
            var y = Fixed64.SqrtFast(xx);
            return (int)(Fixed64.Atan2Fast((long)x << 16, y) >> 16);
        }

        public static int AsinFastest(int x) {
            // Return 0 for invalid values
            if (x < -One || x > One) {
                FixedUtil.InvalidArgument("Fixed32.AsinFastest", "x", x);
                return 0;
            }

            // Compute Atan2(x, Sqrt((1+x) * (1-x))), using s32.32.
            var xx = (long)(One + x) * (long)(One - x);
            var y = Fixed64.SqrtFastest(xx);
            return (int)(Fixed64.Atan2Fastest((long)x << 16, y) >> 16);
        }

        public static int Acos(int x) {
            // Return 0 for invalid values
            if (x < -One || x > One) {
                FixedUtil.InvalidArgument("Fixed32.Acos", "x", x);
                return 0;
            }

            // Compute Atan2(Sqrt((1+x) * (1-x)), x), using s32.32.
            var xx = (long)(One + x) * (long)(One - x);
            var y = Fixed64.Sqrt(xx);
            return (int)(Fixed64.Atan2(y, (long)x << 16) >> 16);
        }

        public static int AcosFast(int x) {
            // Return 0 for invalid values
            if (x < -One || x > One) {
                FixedUtil.InvalidArgument("Fixed32.AcosFast", "x", x);
                return 0;
            }

            // Compute Atan2(Sqrt((1+x) * (1-x)), x), using s32.32.
            var xx = (long)(One + x) * (long)(One - x);
            var y = Fixed64.SqrtFast(xx);
            return (int)(Fixed64.Atan2Fast(y, (long)x << 16) >> 16);
        }

        public static int AcosFastest(int x) {
            // Return 0 for invalid values
            if (x < -One || x > One) {
                FixedUtil.InvalidArgument("Fixed32.AcosFastest", "x", x);
                return 0;
            }

            // Compute Atan2(Sqrt((1+x) * (1-x)), x), using s32.32.
            var xx = (long)(One + x) * (long)(One - x);
            var y = Fixed64.SqrtFastest(xx);
            return (int)(Fixed64.Atan2Fastest(y, (long)x << 16) >> 16);
        }

        [MethodImpl(256)]
        public static int Atan(int x) {
            return Atan2(x, One);
        }

        [MethodImpl(256)]
        public static int AtanFast(int x) {
            return Atan2Fast(x, One);
        }

        [MethodImpl(256)]
        public static int AtanFastest(int x) {
            return Atan2Fastest(x, One);
        }

    }

    #endif

}