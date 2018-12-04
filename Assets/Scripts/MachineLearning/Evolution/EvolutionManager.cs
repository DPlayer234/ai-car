using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DPlay.AICar.MachineLearning.Evolution
{
    /// <summary>
    ///     Handles creating and evolving generations of <see cref="IEvolvable"/>s, always picking the best and creating children based on those.
    ///     Only one instance may exist at once.
    ///     
    ///     Every new generation includes the <seealso cref="ParentCount"/> best cars of the last generation,
    ///     as well as <seealso cref="ChildCount"/> off-spring of those.
    ///     Additional (<seealso cref="TotalCount"/> - <seealso cref="ChildCount"/>) cars are used to add randomness to the system.
    ///     
    ///     The first generation is entirely random.
    /// </summary>
    public class EvolutionManager : MonoBehaviour
    {
        /// <summary> The prefab instantiated per member of a generation. Must contain one <seealso cref="IEvolvable"/> Component. </summary>
        public GameObject SpawnPrefab;

        /// <summary> The "spawn point" of the members. Position and rotation of new members is overridden to the one of this <seealso cref="Transform"/>. </summary>
        public Transform SpawnPoint;

        /// <summary> The total amount of members per generation. </summary>
        [Range(20, 200)]
        public int TotalCount = 20;

        /// <summary> The amount of children of the best members of the generation. </summary>
        [Range(10, 100)]
        public int ChildCount = 10;

        /// <summary> The amount of members whose genomes will be used to create new members. </summary>
        [Range(2, 10)]
        public int ParentCount = 4;

        /// <summary> Mutation chance per gene per individium. </summary>
        [Range(0.0f, 1.0f)]
        public double MutationChance = 0.05f;

        /// <summary> The minimum mutation addition possible. </summary>
        [Range(-1.0f, 0.0f)]
        public double MinimumMutation = -0.1f;

        /// <summary> The maximum mutation addition possible. </summary>
        [Range(0.0f, 1.0f)]
        public double MaximumMutation = 0.1f;

        /// <summary> The time in seconds after which a new generation is started, even if not all members of the current one have become inactive. </summary>
        [Range(10.0f, 300.0f)]
        public float MaximumGenerationAge = 20.0f;

        /// <summary> The current age of the generation in seconds. </summary>
        public float GenerationAge;

        /// <summary> This transform is the parent of all members of the current generation. </summary>
        private Transform generationRoot;

        /// <summary>
        ///     The current active instance of the <seealso cref="EvolutionManager"/>.
        /// </summary>
        public static EvolutionManager Instance { get; private set; }

        /// <summary>
        ///     The List of all <see cref="IEvolvable"/>s of the current generation.
        /// </summary>
        public List<IEvolvable> CurrentGeneration { get; private set; }

        /// <summary>
        ///     The best genomes of the last generation. May be null.
        /// </summary>
        public double[][] BestGenomeOfLastGeneration { get; private set; }

        /// <summary>
        ///     The amount of generation that have already passed.
        /// </summary>
        public int GenerationIndex { get; private set; }

        /// <summary>
        ///     Generates the first generation.
        /// </summary>
        public void GenerateFirstGeneration()
        {
            DestroyGeneration();
            CurrentGeneration = GenerateBaseGeneration();

            GenerationAge = 0.0f;
        }

        /// <summary>
        ///     Generates the next generation based on the current one and input parameters.
        /// </summary>
        public void GenerateNextGeneration()
        {
            // Pick the best genes
            double[][] bestGenomes = GetBestGenomes(ParentCount);

            // Save the best genomes
            BestGenomeOfLastGeneration = bestGenomes;

            // Cross those genes for new cars
            double[][] newGenomes = CrossGenomes(ChildCount, bestGenomes);

            // Override the new generation's genes
            DestroyGeneration();
            CurrentGeneration = GenerateBaseGeneration();

            GenerationAge = 0.0f;
            GenerationIndex++;

            for (int i = 0; i < newGenomes.Length; i++)
            {
                CurrentGeneration[i].SetGenome(newGenomes[i]);
            }
        }

        /// <summary>
        ///     Destroys the current generation.
        /// </summary>
        public void DestroyGeneration()
        {
            if (generationRoot != null)
            {
                Destroy(generationRoot.gameObject);
            }
            
            generationRoot = new GameObject("GenerationRoot").transform;
            generationRoot.parent = transform;
        }

        /// <summary>
        ///     Crosses the given genomes to create new ones.
        /// </summary>
        /// <param name="childCount">The amount of new genomes to generate. Always includes the all parent genomes.</param>
        /// <param name="parentGenomes">The parent genomes.</param>
        /// <returns>An array of new genomes.</returns>
        /// <exception cref="ArgumentException">Less than 2 or more than childCount parents.</exception>
        /// <exception cref="ArgumentException">Not all parent genomes have the same length.</exception>
        public double[][] CrossGenomes(int childCount, double[][] parentGenomes)
        {
            // Make sure we have at least two parents
            int parentCount = parentGenomes.Length;

            if (parentCount < 2 || parentCount >= childCount)
            {
                throw new ArgumentException("Expected at least two and less than childCount parents.", nameof(parentGenomes));
            }

            // Make sure all parents have the same amount of genes, even if this should be a given.
            int geneCount = parentGenomes[0].Length;

            for (int i = 1; i < parentCount; i++)
            {
                if (parentGenomes[i].Length != geneCount)
                {
                    throw new ArgumentException("All parents need to have the same amount of genes.", nameof(parentGenomes));
                }
            }

            double[][] childGenomes = new double[childCount][];

            // Add the parents still.
            for (int i = 0; i < parentCount; i++)
            {
                childGenomes[i] = MutateGenome(parentGenomes[i].CopyArray());
            }

            // Generate the children...
            for (int i = parentCount; i < childCount; i++)
            {
                double[] childGenome = new double[geneCount];

                // ... by randomly assigning gene #g from a random parent.
                for (int g = 0; g < geneCount; g++)
                {
                    childGenome[g] = parentGenomes[Globals.Random.Next(parentCount)][g];
                }

                MutateGenome(childGenome);

                childGenomes[i] = childGenome;
            }

            return childGenomes;
        }

        /// <summary>
        ///     Mutates a genome based on parameters. Does not create a new array but modifies the existing one.
        /// </summary>
        /// <param name="genome">The genome to mutate.</param>
        /// <returns>The original array.</returns>
        public double[] MutateGenome(double[] genome)
        {
            for (int i = 0; i < genome.Length; i++)
            {
                if (Globals.Random.NextDouble() < MutationChance)
                {
                    genome[i] += GetMutationAdder();
                }
            }

            return genome;
        }

        /// <summary>
        ///     Gets the best genomes of the current generation.
        /// </summary>
        /// <param name="amount">The amount of genomes to return.</param>
        /// <returns>An array of the best genomes.</returns>
        /// <exception cref="ArgumentOutOfRangeException">The amount is greater than the generation members.</exception>
        public double[][] GetBestGenomes(int amount)
        {
            if (amount > CurrentGeneration.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), "The amount cannot be greater than the current generation members.");
            }

            CurrentGeneration.Sort();

            double[][] bestGenomes = new double[amount][];

            for (int i = 0; i < amount; i++)
            {
                bestGenomes[i] = CurrentGeneration[i].GetGenome();
            }

            return bestGenomes;
        }

        /// <summary>
        ///     Gets the single best genome of the current generation.
        /// </summary>
        /// <returns>The best genome.</returns>
        public double[] GetBestGenome()
        {
            return GetBestGenomes(1)[0];
        }

        /// <summary>
        ///     Gets a random value in the range [<seealso cref="MinimumMutation"/> .. <seealso cref="MaximumMutation"/>] to be added as mutation to a gene.
        /// </summary>
        /// <returns>A random value in the range [<seealso cref="MinimumMutation"/> .. <seealso cref="MaximumMutation"/>]</returns>
        public double GetMutationAdder()
        {
            double difference = MaximumMutation - MinimumMutation;
            return Globals.Random.NextDouble() * difference - difference * 0.5;
        }

        /// <summary>
        ///     (Empty Default Behaviour) Initializes additional fields of new generations' members.
        /// </summary>
        /// <param name="gameObject">The <seealso cref="GameObject"/> to initialize.</param>
        protected virtual void InitializeChild(GameObject gameObject) { }

        /// <summary>
        ///     Generates members for a new generation.
        /// </summary>
        /// <returns>A list of <seealso cref="IEvolvable"/>s for the next generation.</returns>
        private List<IEvolvable> GenerateBaseGeneration()
        {
            List<IEvolvable> generation = new List<IEvolvable>(TotalCount);

            for (int i = 0; i < TotalCount; i++)
            {
                var gameObject = Instantiate(SpawnPrefab, SpawnPoint.position, SpawnPoint.rotation, generationRoot);
                InitializeChild(gameObject);

                generation.Add(gameObject.GetComponent<IEvolvable>());
            }

            return generation;
        }

        /// <summary>
        ///     Called by Unity to update the <seealso cref="EvolutionManager"/> each frame.
        /// </summary>
        protected virtual void Update()
        {
            if (CurrentGeneration == null || generationRoot == null) return;

            GenerationAge += Time.deltaTime;

            bool anyActive = false;

            foreach (var evolvable in CurrentGeneration)
            {
                if (evolvable.enabled)
                {
                    anyActive = true;
                    break;
                }
            }

            if (!anyActive || GenerationAge > MaximumGenerationAge)
            {
                GenerateNextGeneration();
            }
        }

        /// <summary>
        ///     Called by Unity once to initialize the <seealso cref="EvolutionManager"/>.
        /// </summary>
        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("An old Instance of EvolutionManager was already active.");
                Destroy(Instance);
            }

            Instance = this;

            if (TotalCount < ChildCount)
            {
                Debug.LogWarning("TotalCount has to be at least ChildCount!");
                TotalCount = ChildCount;
            }
        }

        /// <summary>
        ///     Called by Unity once after every Awake() to initialize the <seealso cref="EvolutionManager"/>.
        /// </summary>
        private void Start()
        {
            GenerationIndex = 0;

            GenerateFirstGeneration();
        }
        
        /// <summary>
        ///     Called by Unity when any values of the <seealso cref="EvolutionManager"/> are changed in the editor.
        /// </summary>
        private void OnValidate()
        {
            if (SpawnPrefab != null && SpawnPrefab.GetComponent<IEvolvable>() == null)
            {
                SpawnPrefab = null;

                Debug.LogError("Cannot use this prefab since it does not contain an IEvolvable component.");
            }
        }
    }
}
