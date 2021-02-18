using System;
using System.Collections.Generic;
using System.Linq;

namespace HalloLinq
{
    delegate void EinfacherDelegate(); //Action
    delegate void DelegateMitPara(string text); //Action<string>
    delegate long CalcDelegate(int a, int b); //Func<int, int, long>

    class HalloDelegate
    {
        public HalloDelegate()
        {
            EinfacherDelegate meinDele = EinfacheMethode;
            Action meinDeleAlsAction = EinfacheMethode;
            Action meinDeleAlsActionAno = delegate () { Console.WriteLine("Hallo"); };
            meinDeleAlsActionAno.Invoke();
            Action meinDeleAlsActionAno2 = () => { Console.WriteLine("Hallo"); };
            Action meinDeleAlsActionAno3 = () => Console.WriteLine("Hallo");

            DelegateMitPara deleMitPara = MethodeMitText;
            deleMitPara.Invoke("Hallo");
            Action<string> deleMitPara2 = MethodeMitText;
            Action<string> deleMitParaAno = delegate (string txt) { Console.WriteLine(txt); };
            Action<string> deleMitParaAno2 = (string txt) => { Console.WriteLine(txt); };
            Action<string> deleMitParaAno3 = (string txt) => Console.WriteLine(txt);
            Action<string> deleMitParaAno4 = (txt) => Console.WriteLine(txt);
            Action<string> deleMitParaAno5 = x => Console.WriteLine(x);


            CalcDelegate calc = Minus;
            long result = calc.Invoke(5, 6);
            Func<int, int, long> calcAlsFunc = Sum;
            Func<int, int, long> calcAlsFuncAno = delegate (int x, int y) { return x + y; };
            Func<int, int, long> calcAlsFuncAno2 = (int x, int y) => { return x + y; };
            Func<int, int, long> calcAlsFuncAno3 = (x, y) => { return x + y; };
            Func<int, int, long> calcAlsFuncAno4 = (x, y) => x + y;


            List<string> text = new List<string>();
            text.Where(x => x.StartsWith("B"));
            text.Where(Filter);
            
        }

        bool Filter(string txt)
        {
            if (txt.StartsWith("B"))
                return true;
            else
                return false;
        }

        long Sum(int a, int b)
        {
            return a + b;
        }

        long Minus(int a, int b)
        {
            return a - b;
        }

        void MethodeMitText(string msg)
        {
            Console.WriteLine(msg);
        }

        void EinfacheMethode()
        {
            Console.WriteLine("Hallo");

            var result = Spass();
        }


        (int, long, DateTime) Spass()
        {
            return (5, 347733434, DateTime.Now);
        }
    }
}
