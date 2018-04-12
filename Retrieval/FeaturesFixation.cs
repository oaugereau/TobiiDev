using AnalazingEyeGaze;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retrieval
{
    public class FeaturesFixation
    {

        private List<Gaze> gazes;

        public FeaturesFixation(List<Gaze> gazes)
        {
            this.gazes = gazes; 
        }

        
    }
}
