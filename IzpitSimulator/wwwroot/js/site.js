// Само визуални удобства – оценяването се извършва на сървъра (ExamService.Evaluate).
const form = document.getElementById('sheet');
if (form) {
  const total = +form.dataset.total;
  const progress = document.getElementById('progress');
  const answeredCount = () => new Set([...form.querySelectorAll('input[type=radio]:checked')].map(r => r.name)).size;

  form.addEventListener('change', e => {
    if (e.target.type !== 'radio') return;
    form.querySelectorAll(`input[name="${e.target.name}"]`).forEach(r => r.closest('.opt').classList.toggle('sel', r.checked));
    progress.textContent = `Отговорени: ${answeredCount()}/${total}`;
  });

  form.addEventListener('submit', e => {
    const left = total - answeredCount();
    if (left > 0 && !confirm(`Имате ${left} неотговорени въпроса (те се броят за грешни). Да приключа ли теста?`)) e.preventDefault();
  });
}
