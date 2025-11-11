import ArrowDropDownIcon from "@mui/icons-material/ArrowDropDown";
import ArrowDropUpIcon from "@mui/icons-material/ArrowDropUp";
import {
  Box,
  LinearProgress,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TablePagination,
  TableRow,
  Paper,
  alpha,
} from "@mui/material";
import {
  ColumnDef,
  flexRender,
  getCoreRowModel,
  SortingState,
  useReactTable,
} from "@tanstack/react-table";
import { t } from "i18next";
import React from "react";

type Props<T> = {
  data?: Array<T>;
  columns: Array<ColumnDef<T, any>>;
  loading: boolean;
  page?: number;
  pageSize?: number;
  count?: number;
  handlePageChange?: (page: number) => void;
  handlePageSizeChange?: (page: number) => void;
  sorting?: SortingState;
  setSorting?: React.Dispatch<React.SetStateAction<SortingState>>;
};

const BasicTable = <T extends object>(props: Props<T>) => {
  const table = useReactTable({
    columns: props.columns,
    data: props.data || [],
    getCoreRowModel: getCoreRowModel(),
    onSortingChange: props.setSorting,
    state: {
      sorting: props.sorting,
    },
    enableSortingRemoval: false,
  });

  return (
    <Box>
      <TableContainer
        component={Paper}
        sx={{
          backgroundColor: "background.paper",
          p: 1,
          borderRadius: 0,
          border: (theme) => `1px solid ${alpha(theme.palette.divider, 0.2)}`,
          boxShadow: (theme) => theme.shadows[2],
        }}
      >
        {props.loading ? <LinearProgress sx={{ width: "100%" }} /> : null}
        <Table
          size="small"
          sx={{
            overflowX: "auto",
            width: "100%",
            minWidth: 300,
            borderSpacing: "0px 4px",
          }}
        >
          <TableHead
            sx={{
              borderBottom: (theme) => `2px solid ${theme.palette.divider}`,
              backgroundColor: (theme) =>
                alpha(theme.palette.primary.main, 0.08),
            }}
          >
            {table.getHeaderGroups().map((headerGroup) => (
              <TableRow key={headerGroup.id}>
                {headerGroup.headers.map((header) => (
                  <TableCell
                    key={header.id}
                    sx={{
                      py: 1,
                      px: 2,
                      color: "text.primary",
                      fontWeight: 600,
                      ":hover": {
                        cursor: header.column.getCanSort()
                          ? "pointer"
                          : undefined,
                        backgroundColor: (theme) =>
                          header.column.getCanSort()
                            ? alpha(theme.palette.action.hover, 0.5)
                            : undefined,
                      },
                      border: "none",
                    }}
                    padding={header.id === "actions" ? "checkbox" : "none"}
                    onClick={header.column.getToggleSortingHandler()}
                  >
                    <Box sx={{ display: "flex", alignItems: "center" }}>
                      {header.isPlaceholder
                        ? null
                        : flexRender(
                            header.column.columnDef.header,
                            header.getContext()
                          )}
                      {header.column.getCanSort() &&
                      !header.column.getIsSorted()
                        ? undefined
                        : {
                            asc: <ArrowDropUpIcon />,
                            desc: <ArrowDropDownIcon />,
                          }[header.column.getIsSorted() as string] ?? null}
                    </Box>
                  </TableCell>
                ))}
              </TableRow>
            ))}
          </TableHead>
          <TableBody>
            {table.getRowModel().rows.map((row) => (
              <TableRow
                key={row.id}
                sx={{
                  borderBottom: (theme) =>
                    `1px solid ${alpha(theme.palette.divider, 0.1)}`,
                  "&:hover": {
                    backgroundColor: "action.hover",
                  },
                  "&:last-child": {
                    borderBottom: "none",
                  },
                }}
              >
                {row.getVisibleCells().map((cell) => (
                  <TableCell
                    key={cell.id}
                    padding={cell.column.id === "actions" ? "checkbox" : "none"}
                    sx={{
                      py: 1,
                      px: 2,
                      color: "text.primary",
                    }}
                  >
                    {flexRender(cell.column.columnDef.cell, cell.getContext())}
                  </TableCell>
                ))}
              </TableRow>
            ))}
          </TableBody>
        </Table>
        {props.page &&
        props.pageSize &&
        props.handlePageChange &&
        props.handlePageSizeChange ? (
          <TablePagination
            sx={{
              "& .MuiToolbar-root": {
                flexWrap: "wrap",
                justifyContent: "end",
                color: "text.primary",
              },
              color: "text.primary",
              "& .MuiTablePagination-selectLabel, & .MuiTablePagination-displayedRows":
                {
                  color: "text.secondary",
                },
            }}
            rowsPerPageOptions={[5, 10, 25, 50, 100]}
            component="div"
            count={props.count || 0}
            labelRowsPerPage={t("rowsPerPage") || "Rows per page"}
            labelDisplayedRows={({ from, to, count }) =>
              `${from} - ${to} z ${count}`
            }
            rowsPerPage={props.pageSize}
            page={props.page - 1}
            onPageChange={(_, page) =>
              props.handlePageChange && props.handlePageChange(page + 1)
            }
            onRowsPerPageChange={(e) =>
              props.handlePageSizeChange &&
              props.handlePageSizeChange(parseInt(e.target.value))
            }
          />
        ) : null}
      </TableContainer>
    </Box>
  );
};

export default BasicTable;
