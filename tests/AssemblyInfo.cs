// Фикстуры выполняются параллельно; тесты не зависят друг от друга,
// потому что каждый создаёт собственные данные с уникальным email.
[assembly: Parallelizable(ParallelScope.Fixtures)]
[assembly: LevelOfParallelism(4)]
