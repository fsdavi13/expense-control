import { useEffect, useState } from "react";
import { TransactionForm } from "../components/TransactionForm";
import { TransactionList } from "../components/TransactionList";
import { personService } from "../services/personService";
import { transactionService } from "../services/transactionService";
import type { Person } from "../types/person";
import type {
  CreateTransaction,
  Transaction,
} from "../types/transaction";
import { getErrorMessage } from "../utils/getErrorMessage";
import "./transactions.css";

export function TransactionsPage() {
  const [persons, setPersons] = useState<Person[]>([]);
  const [transactions, setTransactions] = useState<
    Transaction[]
  >([]);
  const [isLoading, setIsLoading] = useState(true);
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [error, setError] = useState("");
  const [successMessage, setSuccessMessage] = useState("");

  useEffect(() => {
    let isCancelled = false;

    Promise.all([
      personService.getAll(),
      transactionService.getAll(),
    ])
      .then(([personsData, transactionsData]) => {
        if (!isCancelled) {
          setPersons(personsData);
          setTransactions(transactionsData);
        }
      })
      .catch((requestError: unknown) => {
        if (!isCancelled) {
          setError(
            getErrorMessage(
              requestError,
              "Não foi possível carregar as transações.",
            ),
          );
        }
      })
      .finally(() => {
        if (!isCancelled) {
          setIsLoading(false);
        }
      });

    return () => {
      isCancelled = true;
    };
  }, []);

  async function handleCreate(
    transaction: CreateTransaction,
  ): Promise<boolean> {
    try {
      setIsSubmitting(true);
      setError("");
      setSuccessMessage("");

      const createdTransaction =
        await transactionService.create(transaction);

      setTransactions((currentTransactions) => [
        ...currentTransactions,
        createdTransaction,
      ]);

      setSuccessMessage(
        "Transação cadastrada com sucesso.",
      );

      return true;
    } catch (requestError) {
      setError(
        getErrorMessage(
          requestError,
          "Não foi possível cadastrar a transação.",
        ),
      );

      return false;
    } finally {
      setIsSubmitting(false);
    }
  }

  return (
    <div className="page">
      <header className="page-header">
        <div>
          <span className="eyebrow">Controle residencial</span>
          <h1>Transações</h1>
          <p>
            Registre receitas e despesas vinculadas às pessoas
            cadastradas.
          </p>
        </div>

        <div className="header-summary">
          <span>Total registrado</span>
          <strong>{transactions.length}</strong>
        </div>
      </header>

      {error && (
        <div className="feedback feedback-error" role="alert">
          {error}
        </div>
      )}

      {successMessage && (
        <div className="feedback feedback-success" role="status">
          {successMessage}
        </div>
      )}

      {isLoading ? (
        <section className="card loading-card">
          <p>Carregando transações...</p>
        </section>
      ) : (
        <div className="transaction-content-grid">
          <TransactionForm
            persons={persons}
            isSubmitting={isSubmitting}
            onSubmit={handleCreate}
          />

          <TransactionList
            transactions={transactions}
            persons={persons}
          />
        </div>
      )}
    </div>
  );
}