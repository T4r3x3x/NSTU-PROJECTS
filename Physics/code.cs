using System;


namespace PhysicsCompute
{
    class Program
    {
        struct converter
        {
            public int type;
            public int mid;
            public float k;
        };
        struct chamber
        {
            public int name; //0 - батарейка, остальная нумирация от минуса батарейки к плюсу всех последующих участков модели
            public float t;
        };
        struct e_circuit
        {
            public float R;
            public float a;
            public float k;
            public float k_e;
            public float ro;
            public float s;
            public float c;
            public float m;
            public float sp;
        };
        void converter_gen(converter[] CV, float[] Rr, e_circuit[] e)
        {
            int[] type = { 0, 6, 28, 33, 21, 45, 47 };//имена участков сопротивлений
            for (int i = 0; i < 50; i++)
            {
                CV[i].type = 7;
                CV[i].k = 1;
                if (i >= 10 && i <= 16) { CV[i].k = Rr[3] / (Rr[3] + Rr[2]); }
                if (i >= 18 && i <= 24) { CV[i].k = (Rr[2] / (Rr[2] + Rr[3])) * (Rr[8] / (e[6].R + Rr[7] + Rr[8])); }
                if (i >= 25 && i <= 31) { CV[i].k = (Rr[2] / (Rr[2] + Rr[3])) * (Rr[6] / (e[6].R + Rr[7] + Rr[8])); }
                if (i >= 32 && i <= 34) { CV[i].k = (Rr[2] / (Rr[2] + Rr[3])) * (Rr[7] / (e[6].R + Rr[7] + Rr[8])); }
                if (i >= 44 && i <= 48) { CV[i].k = Rr[4] / (Rr[4] + Rr[5]); }
                if (i >= 37 && i <= 43) { CV[i].k = Rr[5] / (Rr[5] + Rr[4]); }
                if (i == 17 || i == 35) { CV[i].k = Rr[2] / (Rr[2] + Rr[3]); }
            }
            for (int i = 0; i < 7; i++)
            {
                CV[type[i]].type = i;
            }
        }
        void generation(int acc, chamber[] C, float tc, converter[] CV)
        {
            int[] a = { 0, 1, 8, 23, 25, 35, 56, 66, 100, 110, 114, 148, 153, 164, 179, 187, 192, 226, 231, 246, 256, 260, 270, 274, 284, 299, 314, 324, 328, 338, 342, 352, 367, 376, 386, 395, 401, 424, 448, 458, 482, 497, 523, 533, 557, 578, 588, 596, 606, 632, 638 };
            for (int i = 0; i < 50; i++)
            {
                for (int j = a[i] * acc; j < a[i + 1] * acc; j++)
                {
                    C[j].name = i;
                    C[j].t = tc;
                }
                CV[i].mid = (a[i + 1] * acc - a[i] * acc) / 2;
            }
        }
        float single_heat_exchange(int v1, int v2, e_circuit[] e, converter[] CV, chamber[] C, int acc, ref float t_max, float t)
        {
            float s = e[CV[C[v1].name].type].s, k = e[CV[C[v1].name].type].k, Q;
            if (e[CV[C[v1].name].type].s > e[CV[C[v2].name].type].s) { s = e[CV[C[v2].name].type].s; } //проверка на минимальную площадь соприкосновения
            if (e[CV[C[v1].name].type].k > e[CV[C[v2].name].type].k) { s = e[CV[C[v2].name].type].k; } //проверка на минимальный каэффициент теплопередачи
            Q = (t * k * s * (C[v1].t - C[v2].t)); //тепла передалось следующей вершине
            if (C[v2].t + Q / (e[CV[C[v2].name].type].m * e[CV[C[v2].name].type].c) > C[v1].t)//если теплоты больше, чтобы нагреть вершину на температуру большуу, чем температура v1
            {
                Q = e[CV[C[v2].name].type].m * e[CV[C[v2].name].type].c * (C[v1].t - C[v2].t);
            }
            if ((C[v2].t > C[v1].t) && (C[v1].t + Q / (e[CV[C[v1].name].type].m * e[CV[C[v1].name].type].c) > C[v2].t))//если теплоты больше, чтобы нагреть вершину на температуру большуу, чем температура v2
            {
                Q = e[CV[C[v1].name].type].m * e[CV[C[v1].name].type].c * (C[v2].t - C[v1].t);
            }
            C[v2].t += Q / (e[CV[C[v2].name].type].m * e[CV[C[v2].name].type].c); //теплообмен между этой вершиной и следующей
            if (t_max < C[v2].t) { t_max = C[v2].t; }
            return Q;
        }
        void single_vertex_processing(int v, int v1, int v2, int v3, e_circuit[] e, converter[] CV, float[] Rr, int acc, chamber[] C, ref float t_max, ref float t_mid, ref float Q, float t_environment, float eds, float t)
        {
            float QQ;
            QQ = e[CV[C[v].name].type].sp * t * (C[v].t - t_environment) * e[CV[C[v].name].type].k_e / 2;//количество теплоты, отданное среде
            Q += QQ;//добавляем количество теплоты, отданное среде, в общий счётчик
            QQ += single_heat_exchange(v, v1, e, CV, C, acc, ref t_max, t);//теплообмен между этой вершиной и v1
            if (v2 != -1) { QQ += single_heat_exchange(v, v2, e, CV, C, acc, ref t_max, t); }//теплообмен между этой вершиной и v2, если она указана
            if (v3 != -1) { QQ += single_heat_exchange(v, v3, e, CV, C, acc, ref t_max, t); }//теплообмен между этой вершиной и v3, если она указана
            C[v].t -= QQ / (e[CV[C[v].name].type].c * e[CV[C[v].name].type].m);//температура на вершине после теплообмена
            if (t_max < C[v].t) { t_max = C[v].t; }//проверка на максимальную температуру в цепи на этой вершине после теплообмена
            t_mid += C[v].t;//суммирование для расчёта средней температуры
        }
        void electron_movement(int v_begin, int v_end, bool r, ref float Q, ref float t_mid, float[] Rr, e_circuit[] e, converter[] CV, float t_environment, float t, float eds, int acc, chamber[] C, ref float t_max)
        {
            v_begin = v_begin * acc;
            v_end = v_end * acc;
            if (r == false)
            {
                for (int i = v_begin - 2; i > v_end; i--)
                {
                    single_vertex_processing(i, i - 1, -1, -1, e, CV, Rr, acc, C, ref t_max, ref t_mid, ref Q, t_environment, eds, t);
                }
            }
            else
            {
                for (int i = v_begin - 2; i < v_end; i++)
                {
                    single_vertex_processing(i, i + 1, -1, -1, e, CV, Rr, acc, C, ref t_max, ref t_mid, ref Q, t_environment, eds, t);
                }
            }
        }
        void triple_intersection(int v1, int v2, int v3, bool r, bool output, ref float Q, ref float t_mid, float[] Rr, e_circuit[] e, converter[] CV, float t_environment, float t, float eds, int acc, chamber[] C, ref float t_max)
        {
            if (r == false && output == true)//обход по направлению тока, на перекрётске движение из 2-х направлений
            {
                single_vertex_processing(v1, v2, v3, -1, e, CV, Rr, acc, C, ref t_max, ref t_mid, ref Q, t_environment, eds, t);//теплота, выделившаяся на v1 и её теплообмен с v2 и v3
                single_vertex_processing(v2, v2 - 1, -1, -1, e, CV, Rr, acc, C, ref t_max, ref t_mid, ref Q, t_environment, eds, t);//теплота, выделившаяся на v2 и её теплообмен с v2-1
                single_vertex_processing(v3, v3 - 1, -1, -1, e, CV, Rr, acc, C, ref t_max, ref t_mid,ref Q, t_environment, eds, t);//теплота, выделившаяся на v3 и её теплообмен с v3-1
            }
            if (r == true && output == true)//обход по направлению движения электронов, на перекрётске движение из 2-х направлений
            {
                single_vertex_processing(v1, v2, v1 + 1, -1, e, CV, Rr, acc, C, ref t_max, ref t_mid, ref Q, t_environment, eds, t);//теплота, выделившаяся на v1 и её теплообмен с v2 и v1 + 1
                single_vertex_processing(v2, v2 + 1, -1, -1, e, CV, Rr, acc, C, ref t_max, ref t_mid, ref Q, t_environment, eds, t);//теплота, выделившаяся на v2 и её теплообмен с v2-1
                single_vertex_processing(v1 + 1, v1 + 2, -1, -1, e, CV, Rr, acc, C, ref t_max, ref t_mid, ref Q, t_environment, eds, t);//теплота, выделившаяся на v3 и её теплообмен с v3-1
            }
            if (r == false && output == false)// обход по направлению тока, на перекрётске движение из 1-го направления
            {
                single_vertex_processing(v1, v3, -1, -1, e, CV, Rr, acc, C, ref t_max, ref t_mid, ref Q, t_environment, eds, t);
                single_vertex_processing(v3, v3 - 1, -1, -1, e, CV, Rr, acc, C, ref t_max, ref t_mid, ref Q, t_environment, eds, t);
            }
            if (r == false && output == false)// обход по направлению движения электронов, на перекрётске движение из 1-го направления
            {
                single_vertex_processing(v1, v1 + 1, -1, -1, e, CV, Rr, acc, C, ref t_max, ref t_mid, ref Q, t_environment, eds, t);
                single_vertex_processing(v1 + 1, v1 + 2, -1, -1, e, CV, Rr, acc, C, ref t_max, ref t_mid, ref Q, t_environment, eds, t);
            }
        }
        void quadruple_intersection(int v1, int v2, int v3, int v4, bool r, bool output, ref float Q, ref float t_mid, float[] Rr, e_circuit[] e, converter[] CV, float t_environment, float t, float eds, int acc, chamber[] C, ref float t_max)
        {
            int[] v = { v1, v2, v3, v4 };
            if (r == false && output == true)//обход по направлению тока, на перекрётске движение из 3-х направлений
            {
                single_vertex_processing(v1, v2, v3, v4, e, CV, Rr, acc, C, ref t_max, ref t_mid, ref Q, t_environment, eds, t);
                for (int i = 1; i < 4; i++)
                {
                    single_vertex_processing(v[i], v[i - 1] - 1, -1, -1, e, CV, Rr, acc, C, ref t_max, ref t_mid, ref Q, t_environment, eds, t);
                }
            }
            if (r == true && output == true)//обход по направлению движения электронов, на перекрётске движение из 3-х направлений
            {
                single_vertex_processing(v1, v1 + 1, v2, v3, e, CV, Rr, acc, C, ref t_max, ref t_mid, ref Q, t_environment, eds, t);
                single_vertex_processing(v1 + 1, v1 + 2, -1, -1, e, CV, Rr, acc, C, ref t_max, ref t_mid, ref Q, t_environment, eds, t);
                single_vertex_processing(v2, v2 - 1, -1, -1, e, CV, Rr, acc, C, ref t_max, ref t_mid, ref Q, t_environment, eds, t);
                single_vertex_processing(v3, v3 - 1, -1, -1, e, CV, Rr, acc, C, ref t_max, ref t_mid, ref Q, t_environment, eds, t);
            }
            if (r == false && output == false)//обход по направлению тока, на перекрётске движение из 1-го направления
            {
                single_vertex_processing(v1, v4, -1, -1, e, CV, Rr, acc, C, ref t_max, ref t_mid, ref Q, t_environment, eds, t);
                single_vertex_processing(v4, v4 - 1, -1, -1, e, CV, Rr, acc, C, ref t_max, ref t_mid, ref Q, t_environment, eds, t);
            }
            if (r == true && output == false)//обход по направлению движения электронов, на перекрётске движение из 1-го направления
            {
                single_vertex_processing(v1, v1 + 1, -1, -1, e, CV, Rr, acc, C, ref t_max, ref t_mid, ref Q, t_environment, eds, t);
                single_vertex_processing(v1 + 1, v1 + 2, -1, -1, e, CV, Rr, acc, C, ref t_max, ref t_mid, ref Q, t_environment, eds, t);
            }
        }
        void impedance(e_circuit[] e, float[] Rr, int acc, chamber[] C)
        {
            for (int i = 1; i < 7; i++)
            {
                e[i].R *= 10 * acc;
            }
            Rr[1] = 55 * acc * e[7].R * (1 + e[7].a * C[30].t) + e[1].R * (1 + e[1].a * C[61].t) + 47 * acc * e[7].R * (1 + e[7].a * C[83].t);
            Rr[6] = 28 * acc * e[7].R * (1 + e[7].a * C[214].t) + e[2].R * (1 + e[2].a * C[333].t) + 28 * acc * e[7].R * (1 + e[7].a * C[352].t);
            Rr[7] = 8 * acc * e[7].R * (1 + e[7].a * C[371].t) + e[3].R * (1 + e[3].a * C[380].t) + 8 * acc * e[7].R * (1 + e[7].a * C[390].t);
            Rr[8] = 28 * acc * e[7].R * (1 + e[7].a * C[246].t) + e[4].R * (1 + e[4].a * C[265].t) + 28 * acc * e[7].R * (1 + e[7].a * C[284].t);
            Rr[2] = 4 * acc * e[7].R * (1 + e[7].a * C[228].t) + Rr[6] * Rr[7] * Rr[8] / (Rr[7] * Rr[8] + Rr[6] * Rr[8] + Rr[6] * Rr[7]) + 5 * acc * e[7].R * (1 + e[7].a * C[397].t);
            Rr[3] = 33 * acc * e[7].R * (1 + e[7].a * C[130].t) + 43 * acc * e[7].R * (1 + e[7].a * C[171].t) + 33 * acc * e[7].R * (1 + e[7].a * C[208].t);
            Rr[4] = 20 * acc * e[7].R * (1 + e[7].a * C[567].t) + e[5].R * (1 + e[5].a * C[583].t) + 7 * acc * e[7].R * (1 + e[7].a * C[591].t) + e[6].R * (1 + e[6].a * C[601].t) + 25 * acc * e[7].R * (1 + e[7].a * C[618].t);
            Rr[5] = 57 * acc * e[7].R * (1 + e[7].a * C[453].t) + 18 * acc * e[7].R * (1 + e[7].a * C[490].t) + 59 * acc * e[7].R * (1 + e[7].a * C[528].t);
            Rr[0] = Rr[1] + Rr[2] + Rr[3] / (Rr[3] + Rr[2]) + 22 * acc * e[7].R * (1 + e[7].a * C[412].t) + Rr[4] * Rr[5] / (Rr[5] + Rr[4]) + 5 * acc * e[7].R * (1 + e[7].a * C[634].t);
            for (int i = 1; i < 7; i++)
            {
                e[i].R = e[i].R / (10 * acc);
            }
        }
        void electricity(ref float t_mid, ref float Q, e_circuit[] e, float[] Rr, converter[] CV, float t_environment, float t, float eds, int acc, chamber[] C, ref float t_max, ref float I,ref  float U1, ref float U2)
        {
            int[] alg = { 0, 1, 0, 0, 1, 0, 1, 0, 2, 0, 0, 0, 2, 0, 0, 1, 0 };
            int[] p = { 638, 632, 632 * acc, 632 * acc - 1, 557 * acc - 1, 632, 557, 557, 424, 557 * acc, 424 * acc, 424 * acc - 1, 424, 401, 401 * acc, 401 * acc - 1, 226 * acc - 1, 401, 395, 395 * acc, 395 * acc - 1, 367 * acc - 1, 299 * acc - 1, 395, 367, 367, 299, 299, 231, 367 * acc, 299 * acc, 231 * acc, 231 * acc - 1, 231, 226, 226, 114, 226 * acc, 114 * acc, 114 * acc - 1, 114, 0 };
            float q;
            bool output_triple = true, output_quadruple = true, r = false;
            for (int i = 0; i < 638 * acc; i++)//нагрев каждого элемента цепи электричеством
            {
                q = (eds * t / (e[0].R + Rr[0])) * CV[C[i].name].k;
                C[i].t += (e[CV[C[i].name].type].R * q * q * CV[C[i].name].k) / (t * e[CV[C[i].name].type].c * e[CV[C[i].name].type].m);//полученная вершиной температура от нагрева электричеством
                if (t_max < C[i].t) { t_max = C[i].t; }
            }
            single_vertex_processing(0, 638 * acc - 1, -1, -1, e, CV, Rr, acc, C,ref t_max,ref t_mid, ref Q, t_environment, eds, t);
            t_mid += C[0].t;
            single_vertex_processing(638 * acc - 1, 638 * acc - 2, -1, -1, e, CV, Rr, acc, C,ref t_max,ref  t_mid,ref  Q, t_environment, eds, t);
            t_mid += C[638 * acc - 1].t;
            int revers = 0, j = 0;
            for (int i = 0; i < 34; i++)//обход
            {
                switch (alg[i - (revers * i) + (33 - i) * revers])
                {
                    case 1:
                        {
                            triple_intersection(p[j - (2 * revers)], p[j + 1 - (2 * revers)], p[j + 2 - (2 * revers)], r, output_triple, ref Q, ref t_mid, Rr, e, CV, t_environment, t, eds, acc, C, ref t_max);
                            output_triple = !output_triple;
                            j += 3 - (6 * revers);
                        }
                        break;
                    case 2:
                        {
                            quadruple_intersection(p[j - (3 * revers)], p[j + 1 - (3 * revers)], p[j + 2 - (3 * revers)], p[j + 3 - (3 * revers)], r, output_quadruple, ref Q, ref t_mid, Rr, e, CV, t_environment, t, eds, acc, C, ref t_max);
                            output_quadruple = !output_quadruple;
                            j += 4 - (8 * revers);
                        }
                        break;
                    default:
                        {
                            electron_movement(p[j - (revers)], p[j + 1 - (revers)], r, ref Q, ref t_mid, Rr, e, CV, t_environment, t, eds, acc, C, ref t_max);
                            j += 2 - (4 * revers);
                        }
                        break;
                }
                if (i == 16)
                {
                    r = !r;
                    revers = 1;
                    j -= 1;
                    single_vertex_processing(0, 1, -1, -1, e, CV, Rr, acc, C,ref t_max,ref t_mid,ref Q, t_environment, eds, t);
                    t_mid += C[0].t;
                    single_vertex_processing(1, 2, -1, -1, e, CV, Rr, acc, C,ref t_max, ref t_mid,ref Q, t_environment, eds, t);
                    t_mid += C[1].t;
                }
            }
            q = (eds * t / (e[0].R + Rr[0])) * CV[C[171].name].k;
            U1 = (q / t) * (Rr[2] + Rr[3]);
            q = (eds * t / (e[0].R + Rr[0])) * CV[C[489].name].k;
            U2 = (q / t) * (Rr[4] + Rr[5]);
            q = (eds * t / (e[0].R + Rr[0])) * CV[C[16].name].k;
            I = q / t;
        }
        void main()
        {

            int acc = 0;
            e_circuit[] e = new e_circuit[8];
            float[] Rr = new float[9];
            converter[] CV = new converter[50];
            float ampermetr, voltmeter1, voltmeter2, I = 0, U1 = 0, U2 = 0, u1 = 0, u2 = 0, ii, t_crit, t_k, time_electron_movement, t_conductor = 0, t_environment = 0, eds = 0, ro_sopr = 0, t_max = 0, t_mid = 0;
            float  timer = 0, Q = 0;
            Console.WriteLine("Введите необходимую точность расчёта (целое число, минимальное значение - 1):\n");
            acc = Console.Read();
            time_electron_movement = 1 / acc;
            Console.WriteLine("Введите начальную температуру проводника, °K:\n");
            t_conductor= float.Parse(Console.ReadLine());
            Console.WriteLine("Введите температуру окружающей среды, °K:\n");
            t_environment = float.Parse(Console.ReadLine());
            Console.WriteLine("Введите значение ЭДС источника, В:\n");
            eds= float.Parse(Console.ReadLine());
            Console.WriteLine("Введите удельное сопротивление проводника, Ом·мм²/м:\n");
            ro_sopr= float.Parse(Console.ReadLine());
            Console.WriteLine("Введите сопротивление источника, Oм:\n");
            e[0].R= float.Parse(Console.ReadLine());
            Console.WriteLine("Введите температурный коэффициент сопротивления проводника (если не знаете - введите 0), °K⁻¹:\n");
            e[7].a=float.Parse(Console.ReadLine());
            Console.WriteLine("Введите температурный коэффициент сопротивления источника (если не знаете - введите 0), °K⁻¹:\n");
            e[0].a=float.Parse(Console.ReadLine());
            Console.WriteLine("Введите коэффициент теплопередачи в проводнике (если не знаете - введите 1), Вт/(м²·°K):\n");
            e[7].k= float.Parse(Console.ReadLine());
            Console.WriteLine("Введите коэффициент теплопередачи между проводником и окружающей средой (если не знаете - введите 1), Вт/(м²·°K):\n");
            e[7].k_e=float.Parse(Console.ReadLine());
            Console.WriteLine("Введите коэффициент теплопередачи между источником тока и окружающей средой (если не знаете - введите 1), Вт/(м²·°K):\n");
            e[0].k_e= float.Parse(Console.ReadLine());
            Console.WriteLine("Введите плотность материала проводника, кг/м³:\n");
            e[7].ro =  float.Parse(Console.ReadLine());
            Console.WriteLine("Введите среднюю плотность источника тока, кг/м³:\n");
            e[0].ro =float.Parse(Console.ReadLine());
            Console.WriteLine("Введите удельную теплоёмкость материала проводника, Дж/(кг·°С):\n");
            e[7].c= float.Parse(Console.ReadLine());
            Console.WriteLine("Введите удельную теплоёмкость источника, Дж/(кг·°С):\n");
            e[0].c= float.Parse(Console.ReadLine());
            Console.WriteLine("Введите площадь поперечного сечения проводника, мм²:\n");
            e[7].s=   float.Parse(Console.ReadLine());
            for (int i = 1; i < 7; i++)
            {
                Console.WriteLine("Введите сопротивление резистора R%d, Oм:\n", i);
                e[i].R=  float.Parse(Console.ReadLine());
                e[i].R = e[i].R / (10 * acc);
                Console.WriteLine("Введите температурный коэффициент сопротивления резистора R%d (если не знаете - введите 0), °K⁻¹:\n", i);
                e[i].a=  float.Parse(Console.ReadLine());
                Console.WriteLine("Введите коэффициент теплопередачи в резисторе R%d (если не знаете - введите 1), Вт/(м²·°K):\n", i);
                e[i].k= float.Parse(Console.ReadLine());
                Console.WriteLine("Введите коэффициент теплопередачи между резистором R%d и окружающей средой (если не знаете - введите 1), Вт/(м²·°K):\n", i);
                e[i].k_e=float.Parse(Console.ReadLine());
                Console.WriteLine("Введите плотность материала резистора R%d, кг/м³:\n", i);
                e[i].ro=float.Parse(Console.ReadLine());
                Console.WriteLine("Введите удельную теплоёмкость материала резистора R%d, Дж/(кг·°С):\n", i);
                e[i].c= float.Parse(Console.ReadLine());
                Console.WriteLine("Введите площадь поперечного сечения резистора R%d, мм²:\n", i);
                e[i].s= float.Parse(Console.ReadLine());
            }
            Console.WriteLine("Введите коэффициент умножения времени (по умолчанию - 1):\nЧем он больше, тем расчёт выполняется быстрее, но грубее,\nчем он меньше - расчёт более точен, но выполняется медленнее.\n");
            t_k= float.Parse(Console.ReadLine());
            time_electron_movement *= t_k;
            Console.WriteLine("Введите цену деления для первого вольтметра (по умолчанию 0,2 вольта), В:\n");
            u1=float.Parse(Console.ReadLine());
            Console.WriteLine("Введите цену деления для второго вольтметра (по умолчанию 0,2 вольта), В:\n");
            u2=float.Parse(Console.ReadLine());
            Console.WriteLine("Введите цену деления для амперметра (по умолчанию 0,2 ампера), А:\n");
            ii=float.Parse(Console.ReadLine());
            Console.WriteLine("Введите критическую температуру в цепи (при которой либо плавится проводник, либо выходят из строя приборы), °K:\n");
            t_crit=float.Parse(Console.ReadLine());
            e[0].k = e[7].k;
            e[0].s = (float)Math.PI * (10f * 10f); //площадь поперечного сечения источника
            e[0].m = e[0].ro * (e[0].s / 1000000f) * 0.07f; //масса источника
            e[0].sp = 0.07f * (2 * (float)Math.PI * 0.01f); //площадь боковой поверхности источника
            for (int i = 0; i < 8; i++)//перевод в метры
            {
                e[i].s /= 1000000;
            }
            chamber[] circuit = new chamber[638 * acc + 1];
            generation(acc, circuit, t_conductor, CV);
            for (int i = 0; i < 1; i++)
            {
                e[7].R = ro_sopr * 0.002f / ((e[7].s * 1000000f) * acc); //сопротивление элементарного участка проводника
                for (int j = 1; j < 8; j++)
                {
                    e[j].m = e[j].ro * (e[j].s) * (0.002f / acc); //масса элементарного участка цепи
                    e[j].sp = (0.002f / acc) * (2f * (float)Math.PI * (float) Math.Sqrt(e[j].s / (float)Math.PI)); //площадь боковой поверхности элементарного участка цепи
                    e[j].R /= (10 * acc);
                }
                impedance(e, Rr, acc, circuit);
                Console.WriteLine("Полное сопротивление цепи равно: %f Ом\n", Rr[0]);
                converter_gen(CV, Rr, e);
                electricity(ref t_mid, ref Q, e, Rr, CV, t_environment, time_electron_movement, eds, acc, circuit, ref t_max, ref I, ref U1, ref U2);
               
                timer = timer + time_electron_movement;
                ampermetr = I * 4 / ii;
                if (I * 4 / ii > 180)
                {
                    Console.WriteLine("Внимание: перегрузка амперметра!\n");
                    ampermetr = 180;
                }
                Console.WriteLine("Показания амперметра (в градусах отклонения стрелки (начальный 0, конечный 180)): %f°\n", ampermetr);
                Console.WriteLine("Показания амперметра (в амперах): %f А\n", I);
                voltmeter1 = U1 * 4 / u1;
                if (U1 * 4 / u1 > 180)
                {
                    Console.WriteLine("Внимание: перегрузка первого вольтметра!\n");
                    voltmeter1 = 180;
                }
                Console.WriteLine("Показания первого вольтметра (в градусах отклонения стрелки (начальный 0, конечный 180)): %f°\n", voltmeter1);
                Console.WriteLine("Показания первого вольтметра (в вольтах): %f В\n", U1);
                voltmeter2 = U2 * 4 / u2;
                if (U2 * 4 / u2 > 180)
                {
                    Console.WriteLine("Внимание: перегрузка второго вольтметра!\n");
                    voltmeter2 = 180;
                }
                Console.WriteLine("Показания второго вольтметра (в градусах отклонения стрелки (начальный 0, конечный 180)): %f°\n", voltmeter2);
                Console.WriteLine("Показания второго вольтметра (в вольтах): %f В\n", U2);
                Console.WriteLine("Зафиксированная максимальная температура: %f°K\n", t_max);
                Console.WriteLine("Средняя температура в цепи: %lf°K\n", t_mid / (638 * 2 * acc));
                Console.WriteLine("Времени от начала работы программы в виртуальном мире самой программы прошло: %lf с\n", timer);
                Console.WriteLine("Теплоты окружающей среде отдано: %lf Дж\n", Q);
                for (int j = 0; j < 50; j++)
                {
                    Console.WriteLine("Температура участка %d: %f°К\n", j, circuit[CV[j].mid].t);
                }
                Console.WriteLine("\n");
            }
        }
    }
}
