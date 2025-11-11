import { useState, useMemo } from "react";

export const usePagination = () => {
  const [pageNumber, setPage] = useState<number>(1);
  const [pageSize, setPageSize] = useState<number>(10);
  const [totalPagesCount, setTotalPagesCount] = useState<number | null>(null);
  const [totalCount, setTotalCount] = useState<number | null>(null);

  const pagination = useMemo(() => {
    return {
      page : {pageNumber, setPage, pageSize, setPageSize},
      total : {totalCount, setTotalCount, totalPagesCount, setTotalPagesCount}
    };
  }, [pageNumber, pageSize, totalPagesCount, totalCount]);

  return useMemo(
    () => ({
      pagination,
      paginationState: {
        pageNumber,
        setPage,
        pageSize,
        setPageSize,
        totalPagesCount,
        setTotalPagesCount,
        totalCount,
        setTotalCount,
      },
    }),
    [pagination, pageNumber, pageSize, totalPagesCount, totalCount]
  );
};