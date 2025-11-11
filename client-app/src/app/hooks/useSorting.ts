import { useMemo, useState } from 'react';

export const useSorting = () => {
  const [sortBy, setSortBy] = useState<string | null>(null);
  const [isDescending,setIsDescending] = useState<boolean>(false);

  return useMemo(
    () => ({
      sortBy: {sortBy, setSortBy},
      isDescending: {isDescending,setIsDescending},
    }),
    [sortBy, isDescending]
  );
};
