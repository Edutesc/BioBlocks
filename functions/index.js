const {onRequest} = require("firebase-functions/v2/https");
const admin = require("firebase-admin");
admin.initializeApp();

// Função HTTP usando explicitamente a API v2
exports.resetWeeklyScores = onRequest(async (req, res) => {
  // Adicionar uma chave de segurança básica
  const secretKey = req.query.key;
  if (secretKey !== "bioblocks2025") { // Altere esta chave para algo seguro
    res.status(403).send("Acesso não autorizado");
    return;
  }

  try {
    const db = admin.firestore();
    const usersRef = db.collection("Users");
    
    // Buscar todos os usuários
    const snapshot = await usersRef.get();
    
    // Criar um batch para atualizações em massa
    const batch = db.batch();
    let count = 0;
    
    snapshot.forEach((doc) => {
      batch.update(doc.ref, {"WeekScore": 0});
      count++;
    });
    
    // Executar o batch
    await batch.commit();
    
    res.status(200).send(`Sucesso! Scores semanais resetados para ${count} usuários.`);
  } catch (error) {
    console.error("Erro ao resetar scores semanais:", error);
    res.status(500).send(`Erro: ${error.message}`);
  }
});