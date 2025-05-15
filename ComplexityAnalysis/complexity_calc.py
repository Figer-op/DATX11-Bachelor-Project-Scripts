import re
import math
import pandas as pd
import textwrap

# --- C# source code ----------------------------------------------------------
code: str = textwrap.dedent("""
public class Test
{
    private String test = "This is a test";
}
""")

# --- Helper data -------------------------------------------------------------
operator_keywords: set[str] = {
    'public','private','protected','class','static','virtual','override',
    'new','return','if','for','while','do','break','continue','else','switch','case',
    'default','namespace','using','try','catch','finally','throw','this',
    'in','out','ref','void','int','float','double','string','bool','object',
    'byte','char','struct','enum','long','short','decimal','const','readonly',
    'get','set','add','remove','params','typeof','sizeof','checked','unchecked',
    'async','await','var','dynamic','is','as'
}
operator_symbols: set[str] = set("+-*/%=&|^~!<>?:;,.({[")

# --- Tokenisation ------------------------------------------------------------
# All C# compound punctuation operators we want to recognise as a *single* token.
multi_char_ops: list[str] = [
    '++', '--', '==', '!=', '<=', '>=',               # arithmetic / comparison
    '+=', '-=', '*=', '/=', '%=', '&=', '|=', '^=',   # assignment‑with‑op
    '&&', '||', '<<', '>>',                           # logical / bit shift
    '<<=', '>>=',                                     # shift‑assignment
    '??', '?.', '??=',                                # null‑coalescing /‑propagation
    '=>', '->'                                        # lambda, pointer deref
]

# Pattern for C# string literals (regular, verbatim, and interpolated)
string_regex: str = r'\$@"[^"]*"|@"[^"]*"|\$"(?:\\.|[^"\\])*"|"(?:\\.|[^"\\])*"'

# Longest first, so ">>=" wins over ">>"
multi_char_ops.sort(key=len, reverse=True)

# Build a single regex:
# - serialize_field for the special case of the various serialize field attributes.
# - \w+ for identifiers / literals
# - multi_ops for the list of operator symbols
# - Lastly a list for every *single* punctuation character that should be treated individually
multi_ops_regex: str = '|'.join(map(re.escape, multi_char_ops))
serialize_field: str = r'\[SerializeField\]|\[System\.Serializable\]|\[field\: SerializeField\]'
token_pattern: str = rf'{string_regex}|{serialize_field}|\w+|{multi_ops_regex}|[+\-*/%=&|^~!<>?:;,.()\{{\}}\[\]]'

tokens: list = re.findall(token_pattern, code)

distinct_operators = set()
distinct_operands = set()
total_operators: list = []
total_operands: list = []
ignored_tokens: list[str] = [')','}',']', '[SerializeField]', '[System.Serializable]', '[field: SerializeField]']

for tok in tokens:
    if tok in operator_symbols or tok in operator_keywords or tok in multi_char_ops:
        distinct_operators.add(tok)
        total_operators.append(tok)
    elif tok not in ignored_tokens:
        # treat identifiers and literals as operands
        distinct_operands.add(tok)
        total_operands.append(tok)

n1: int = len(distinct_operators)
n2: int = len(distinct_operands)
N1: int = len(total_operators)
N2: int = len(total_operands)
# vocabulary
n: int = n1 + n2    
# length
N: int = N1 + N2                       
V: float = 0 if n == 0 else N * math.log2(n)
D: float = (n1/2) * (N2 / n2)
E: float = D * V

metrics = pd.DataFrame({
    "Metric": ["Distinct operators (n₁)",
               "Distinct operands (n₂)",
               "Program vocabulary (n)",
               "Total operators (N₁)",
               "Total operands (N₂)",
               "Program length (N)",
               "Halstead Volume (V)"],
    "Value": [n1, n2, n, N1, N2, N, round(V, 2)]
})

def count_non_blank_lines(code: str) -> int:
    """
    Count the number of lines of code in `code`, ignoring blank lines.

    Parameters
    ----------
    code : str
        A string containing the source code.

    Returns
    -------
    int
        The count of non-blank lines.
    """
    # `splitlines()` works with '\n', '\r\n', or '\r'
    return sum(1 for line in code.splitlines() if line.strip())

def calculate_cyclomatic_metrics(code: str) -> tuple[int, int]:
    """
    Return (P, CC) for the supplied C# source code, where the following decision points are counted:
    - if / else-if
    - for
    - foreach
    - while
    - do-while
    - case
    - catch
    - logical && ||
    - null-coalescing ??
    - ternary ?:

    Parameters
    ----------
    code : str
        A string containing the source code.

    Returns
    -------
    tuple[int, int]
        The (P, CC) for the supplied C# source
        - P is the number of predicate / decision nodes
        - CC is cyclomatic complexity = P + 1
    """
    
    tokens: list = re.findall(token_pattern, code)

    decision_keywords: set[str] = {
        "if", "for", "foreach", "while", "case", "catch", "default"
    }
    decision_operators: set[str] = {"&&", "||", "??", "?", "=>"}          # '?' covers ?: ternary

    P = 0
    i = 0
    while i < len(tokens):
        tok = tokens[i]

        # keyword‑based decisions
        if tok in decision_keywords:
            # skip the 'if' that immediately follows an 'else'
            if not (tok == "if" and i > 0 and tokens[i - 1] == "else"):
                P += 1

        # operator‑based decisions
        elif tok in decision_operators:
            P += 1

        i += 1

    CC: int = P + 1
    return P, CC
P, CC = calculate_cyclomatic_metrics(code)

print(metrics.to_string(index=False))
print("Difficulty:", D)
print("Effort:", E)
print("Decision points (P):", P)
print("Cyclomatic complexity:", CC)
print("Non-blank lines:", count_non_blank_lines(code))
