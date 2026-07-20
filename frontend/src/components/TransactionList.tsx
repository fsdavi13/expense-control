import type { Person } from "../types/person";
import {
  TransactionType,
  type Transaction,
} from "../types/transaction";

interface TransactionListProps {
  transactions: Transaction[];
  persons: Person[];
}

const currencyFormatter = new Intl.NumberFormat("pt-BR", {
  style: "currency",
  currency: "BRL",
});

export function TransactionList({
  transactions,
  persons,
}: TransactionListProps) {
  function getPersonName(personId: number): string {
    return (
      persons.find((person) => person.id === personId)?.name ??
      "Pessoa não encontrada"
    );
  }

  return (
    <section className="card">
      <div className="section-heading">
        <div>
          <span className="eyebrow">Histórico</span>
          <h2>Transações cadastradas</h2>
        </div>

        <span className="counter-badge">
          {transactions.length}
        </span>
      </div>

      {transactions.length === 0 ? (
        <div className="empty-state">
          <strong>Nenhuma transação cadastrada</strong>
          <p>
            Use o formulário para adicionar a primeira
            movimentação.
          </p>
        </div>
      ) : (
        <div className="table-wrapper">
          <table>
            <thead>
              <tr>
                <th>Descrição</th>
                <th>Pessoa</th>
                <th>Tipo</th>
                <th className="value-column">Valor</th>
              </tr>
            </thead>

            <tbody>
              {transactions.map((transaction) => {
                const isIncome =
                  transaction.type === TransactionType.Income;

                return (
                  <tr key={transaction.id}>
                    <td>
                      <div className="transaction-description">
                        <strong>{transaction.description}</strong>
                        <small>ID #{transaction.id}</small>
                      </div>
                    </td>

                    <td>{getPersonName(transaction.personId)}</td>

                    <td>
                      <span
                        className={
                          isIncome
                            ? "transaction-type income"
                            : "transaction-type expense"
                        }
                      >
                        {isIncome ? "Receita" : "Despesa"}
                      </span>
                    </td>

                    <td
                      className={
                        isIncome
                          ? "value-column income-value"
                          : "value-column expense-value"
                      }
                    >
                      {isIncome ? "+" : "-"}{" "}
                      {currencyFormatter.format(
                        transaction.amount,
                      )}
                    </td>
                  </tr>
                );
              })}
            </tbody>
          </table>
        </div>
      )}
    </section>
  );
}