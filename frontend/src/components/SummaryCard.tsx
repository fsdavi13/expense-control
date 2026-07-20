interface SummaryCardProps {
  label: string;
  value: string;
  variant?: "default" | "income" | "expense";
}

export function SummaryCard({
  label,
  value,
  variant = "default",
}: SummaryCardProps) {
  return (
    <article className={`summary-card ${variant}`}>
      <span>{label}</span>
      <strong>{value}</strong>
    </article>
  );
}