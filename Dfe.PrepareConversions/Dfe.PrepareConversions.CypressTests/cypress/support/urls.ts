// Joins a base URL and a path with exactly one slash between them.
// Written with plain string scanning rather than regex quantifiers, which keeps it
// linear-time and avoids the backtracking Sonar flags (S5852).
export const joinApiUrl = (base: string, path: string): string => {
    let end = base.length;
    while (end > 0 && base.charAt(end - 1) === '/') {
        end--;
    }

    let start = 0;
    while (start < path.length && path.charAt(start) === '/') {
        start++;
    }

    return `${base.slice(0, end)}/${path.slice(start)}`;
};