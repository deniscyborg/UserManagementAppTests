// Fixtures run in parallel; tests are independent of each other
// because each one creates its own data with a unique email.
[assembly: Parallelizable(ParallelScope.Fixtures)]
[assembly: LevelOfParallelism(4)]
