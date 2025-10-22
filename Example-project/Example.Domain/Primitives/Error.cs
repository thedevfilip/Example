﻿namespace Example.Domain.Primitives;

public record Error(string Code, string Desription)
{
    public static Error None => new(string.Empty, string.Empty);
}
