using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPlay.AICar.MachineLearning.Evolution
{
    public interface IEvolvable : IComparable<IEvolvable>
    {
        bool Active { get; }

        double Fitness { get; }

        double[] GetGenome();

        void SetGenome(double[] genes);
    }
}
